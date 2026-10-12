using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeam.CreateLeagueTeam;

/// <summary>
/// Represents a request to add a team to the roster of a league.
/// </summary>
public sealed record CreateLeagueTeamRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the league the team joins.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The identifier of the team joining the league.
    /// </summary>
    public int TeamId { get; init; }
}
