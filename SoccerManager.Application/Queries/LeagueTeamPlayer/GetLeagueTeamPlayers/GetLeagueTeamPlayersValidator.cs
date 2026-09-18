using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayers;

/// <summary>
/// Validates <see cref="GetLeagueTeamPlayersRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamPlayersValidator : AbstractValidator<GetLeagueTeamPlayersRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayersValidator"/> class.
    /// </summary>
    public GetLeagueTeamPlayersValidator()
    {
        // This query is always scoped to one team within one league, so a
        // missing or zero LeagueId or TeamId must fail fast rather than
        // silently returning nothing.
        RuleFor(x => x.LeagueId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
    }
}
