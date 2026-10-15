using FluentValidation;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Queries.Player.SearchPlayers;

/// <summary>
/// Validates <see cref="SearchPlayersRequest"/> instances.
/// </summary>
public sealed class SearchPlayersValidator : AbstractValidator<SearchPlayersRequest>
{
    /// <summary>
    /// The column names <see cref="SearchPlayersRequest.SortBy"/> accepts, compared without regard to case.
    /// </summary>
    private static readonly string[] SortByNames = Enum.GetNames<PlayerSortBy>();

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPlayersValidator"/> class.
    /// </summary>
    public SearchPlayersValidator()
    {
        // Matches the NVARCHAR(100) Name column. The procedure parameter would
        // otherwise cut a longer term silently instead of rejecting it.
        RuleFor(x => x.SearchTerm).MaximumLength(100);

        // Shape-only: an unknown position, club or national team simply matches no player.
        RuleFor(x => x.PositionId).GreaterThan(0).When(x => x.PositionId.HasValue);
        RuleFor(x => x.TeamId).GreaterThan(0).When(x => x.TeamId.HasValue);
        RuleFor(x => x.NationalTeamId).GreaterThan(0).When(x => x.NationalTeamId.HasValue);

        // The same range player ratings are written with.
        RuleFor(x => x.MinRating).InclusiveBetween(1, 100).When(x => x.MinRating.HasValue);
        RuleFor(x => x.MaxRating).InclusiveBetween(1, 100).When(x => x.MaxRating.HasValue);
        RuleFor(x => x.MaxRating)
            .GreaterThanOrEqualTo(x => x.MinRating)
            .When(x => x.MinRating.HasValue && x.MaxRating.HasValue);

        // Names only: Enum.Parse would also accept a number, which the contract does not offer.
        RuleFor(x => x.SortBy)
            .Must(sortBy => sortBy is null || SortByNames.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"'{{PropertyName}}' must be one of: {string.Join(", ", SortByNames).ToLowerInvariant()}.");

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).When(x => x.Page.HasValue);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).When(x => x.PageSize.HasValue);
    }
}
