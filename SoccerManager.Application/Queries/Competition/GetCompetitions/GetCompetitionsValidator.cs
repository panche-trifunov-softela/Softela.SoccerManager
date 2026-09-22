using FluentValidation;

namespace SoccerManager.Application.Queries.Competition.GetCompetitions;

/// <summary>
/// Validates <see cref="GetCompetitionsRequest"/> instances.
/// </summary>
public sealed class GetCompetitionsValidator : AbstractValidator<GetCompetitionsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetCompetitionsValidator"/> class.
    /// </summary>
    public GetCompetitionsValidator()
    {
        // Unlike GetLeagues, this query is always scoped to a league, so a missing
        // or zero LeagueId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueId).GreaterThan(0);
    }
}
