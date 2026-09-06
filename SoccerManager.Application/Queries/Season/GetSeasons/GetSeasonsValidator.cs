using FluentValidation;

namespace SoccerManager.Application.Queries.Season.GetSeasons;

/// <summary>
/// Validates <see cref="GetSeasonsRequest"/> instances.
/// </summary>
public sealed class GetSeasonsValidator : AbstractValidator<GetSeasonsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSeasonsValidator"/> class.
    /// </summary>
    public GetSeasonsValidator()
    {
        // Unlike GetLeagues, this query is always scoped to a league, so a missing
        // or zero LeagueId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueId).GreaterThan(0);
    }
}
