using MediatR;

namespace SoccerManager.Application.Commands.PlayerPosition.CreatePlayerPosition;

/// <summary>
/// Represents a request to create a new player position rating.
/// </summary>
public sealed record CreatePlayerPositionRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the player the rating belongs to.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The identifier of the position being rated.
    /// </summary>
    public int PositionId { get; init; }

    /// <summary>
    /// How well the player performs in that position, from 1 to 100.
    /// </summary>
    public int Quality { get; init; }
}
