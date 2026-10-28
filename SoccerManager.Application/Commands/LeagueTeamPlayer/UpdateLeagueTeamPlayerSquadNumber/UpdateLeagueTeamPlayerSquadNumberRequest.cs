using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayerSquadNumber;

/// <summary>
/// Represents a request to change the squad number of an existing league team player.
/// </summary>
public sealed record UpdateLeagueTeamPlayerSquadNumberRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league team player whose squad number to change.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The player's new squad number, from 1 to 99 inclusive.
    /// </summary>
    public int SquadNumber { get; init; }
}
