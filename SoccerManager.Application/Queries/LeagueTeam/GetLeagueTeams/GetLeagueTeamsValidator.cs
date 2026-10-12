using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeams;

/// <summary>
/// Validates <see cref="GetLeagueTeamsRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamsValidator : AbstractValidator<GetLeagueTeamsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamsValidator"/> class.
    /// </summary>
    public GetLeagueTeamsValidator()
    {
        // Unlike GetLeagues, this query is always scoped to a league, so a missing
        // or zero LeagueId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueId).GreaterThan(0);
    }
}
