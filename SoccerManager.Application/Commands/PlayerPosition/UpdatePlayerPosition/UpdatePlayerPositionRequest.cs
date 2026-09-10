using MediatR;

namespace SoccerManager.Application.Commands.PlayerPosition.UpdatePlayerPosition;

/// <summary>
/// Represents a request to update an existing player position rating.
/// </summary>
public sealed record UpdatePlayerPositionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the player position rating to update.
    /// </summary>
    public int Id { get; init; }

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
