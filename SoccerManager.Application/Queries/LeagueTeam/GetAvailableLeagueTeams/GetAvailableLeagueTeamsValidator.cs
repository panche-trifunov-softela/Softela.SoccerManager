using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;

/// <summary>
/// Validates <see cref="GetAvailableLeagueTeamsRequest"/> instances.
/// </summary>
public sealed class GetAvailableLeagueTeamsValidator : AbstractValidator<GetAvailableLeagueTeamsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetAvailableLeagueTeamsValidator"/> class.
    /// </summary>
    public GetAvailableLeagueTeamsValidator()
    {
        // This query is always scoped to a league, so a missing or zero LeagueId
        // must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueId).GreaterThan(0);
    }
}
