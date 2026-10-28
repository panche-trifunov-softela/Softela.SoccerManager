using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquad;

/// <summary>
/// Validates <see cref="GetLeagueTeamSquadRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamSquadValidator : AbstractValidator<GetLeagueTeamSquadRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamSquadValidator"/> class.
    /// </summary>
    public GetLeagueTeamSquadValidator()
    {
        // This query is always scoped to a league team, so a missing or zero LeagueTeamId
        // must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
