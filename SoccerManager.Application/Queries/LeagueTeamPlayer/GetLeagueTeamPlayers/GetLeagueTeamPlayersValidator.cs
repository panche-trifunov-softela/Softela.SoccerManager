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
        // This query is always scoped to one club's roster entry in a league, so a
        // missing or zero LeagueTeamId must fail fast rather than silently
        // returning nothing.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
