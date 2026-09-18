using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.DeleteLeagueTeamPlayer;

/// <summary>
/// Represents a request to delete an existing league team player.
/// </summary>
public sealed record DeleteLeagueTeamPlayerRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league team player to delete.
    /// </summary>
    public int Id { get; init; }
}
