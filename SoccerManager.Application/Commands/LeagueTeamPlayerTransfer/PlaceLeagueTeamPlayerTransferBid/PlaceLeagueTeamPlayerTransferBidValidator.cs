using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.PlaceLeagueTeamPlayerTransferBid;

/// <summary>
/// Validates <see cref="PlaceLeagueTeamPlayerTransferBidRequest"/> instances.
/// </summary>
public sealed class PlaceLeagueTeamPlayerTransferBidValidator : AbstractValidator<PlaceLeagueTeamPlayerTransferBidRequest>
{
    /// <summary>
    /// The most seasons a bid can ask the buyback to stay valid.
    /// </summary>
    private const int MaxBuybackValidSeasons = 2;

    /// <summary>
    /// The most players a bid can offer in a swap.
    /// </summary>
    private const int MaxSwapPlayers = 2;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceLeagueTeamPlayerTransferBidValidator"/> class.
    /// </summary>
    public PlaceLeagueTeamPlayerTransferBidValidator()
    {
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);

        // PrecisionScale matches the DECIMAL(18,2) columns, so an over-precise value is a 400
        // rather than a silent rounding or an overflow at the database.
        RuleFor(x => x.TransferFee).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false);
        RuleFor(x => x.BuybackValue).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false).When(x => x.BuybackValue.HasValue);
        RuleFor(x => x.BuybackValidSeasons).InclusiveBetween(1, MaxBuybackValidSeasons).When(x => x.BuybackValidSeasons.HasValue);

        // Mirrors CK_LeagueTeamPlayerTransfers_BuybackBothOrNeither, so a breach is a 400 rather than a database error.
        RuleFor(x => x.BuybackValue)
            .NotNull()
            .WithMessage("Set the buyback value and its seasons together, or neither.")
            .When(x => x.BuybackValidSeasons.HasValue);

        RuleFor(x => x.BuybackValidSeasons)
            .NotNull()
            .WithMessage("Set the buyback value and its seasons together, or neither.")
            .When(x => x.BuybackValue.HasValue);

        // The chain stops at the first failure, so the later rules read a list that is not null.
        RuleFor(x => x.SwapLeagueTeamPlayerIds)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("The swapped players are required.")
            .Must(ids => ids.Count <= MaxSwapPlayers)
            .WithMessage($"At most {MaxSwapPlayers} players can be offered in a swap.")
            .Must(ids => ids.All(id => id > 0))
            .WithMessage("Every swapped player must be a valid squad player identifier.")
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("A player can be offered in a swap only once.");
    }
}
