using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeam.DeleteLeagueTeam;

/// <summary>
/// Represents a request to remove a team from the roster of a league.
/// </summary>
public sealed record DeleteLeagueTeamRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league team to delete.
    /// </summary>
    public int Id { get; init; }
}
