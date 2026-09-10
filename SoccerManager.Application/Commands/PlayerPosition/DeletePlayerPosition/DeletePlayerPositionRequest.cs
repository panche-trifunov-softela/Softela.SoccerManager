using MediatR;

namespace SoccerManager.Application.Commands.PlayerPosition.DeletePlayerPosition;

/// <summary>
/// Represents a request to delete an existing player position rating.
/// </summary>
public sealed record DeletePlayerPositionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the player position rating to delete.
    /// </summary>
    public int Id { get; init; }
}
