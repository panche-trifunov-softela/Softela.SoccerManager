using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.CreateLeagueTeamPlayerTransfer;

/// <summary>
/// Validates <see cref="CreateLeagueTeamPlayerTransferRequest"/> instances.
/// </summary>
public sealed class CreateLeagueTeamPlayerTransferValidator : AbstractValidator<CreateLeagueTeamPlayerTransferRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamPlayerTransferValidator"/> class.
    /// </summary>
    public CreateLeagueTeamPlayerTransferValidator()
    {
        RuleFor(x => x.LeagueTeamPlayerId).GreaterThan(0);
        RuleFor(x => x.ToLeagueTeamId).GreaterThan(0);
        RuleFor(x => x.SeasonId).GreaterThan(0);

        // PrecisionScale matches the DECIMAL(18,2) columns, so an over-precise value is a 400
        // rather than a silent rounding or an overflow at the database.
        RuleFor(x => x.TransferFee).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false);
        RuleFor(x => x.BuybackValue).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false).When(x => x.BuybackValue.HasValue);
        RuleFor(x => x.BuybackValidSeasons).GreaterThanOrEqualTo(1).When(x => x.BuybackValidSeasons.HasValue);

        // Mirrors CK_LeagueTeamPlayerTransfers_BuybackBothOrNeither, so a breach is a 400 rather than a database error.
        RuleFor(x => x.BuybackValue)
            .NotNull()
            .WithMessage("Set the buyback value and its seasons together, or neither.")
            .When(x => x.BuybackValidSeasons.HasValue);

        RuleFor(x => x.BuybackValidSeasons)
            .NotNull()
            .WithMessage("Set the buyback value and its seasons together, or neither.")
            .When(x => x.BuybackValue.HasValue);

        RuleFor(x => x.Status).IsInEnum();
    }
}
