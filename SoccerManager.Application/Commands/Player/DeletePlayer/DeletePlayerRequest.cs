using MediatR;

namespace SoccerManager.Application.Commands.Player.DeletePlayer;

/// <summary>
/// Represents a request to delete an existing player.
/// </summary>
public sealed record DeletePlayerRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the player to delete.
    /// </summary>
    public int Id { get; init; }
}
