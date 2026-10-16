using MediatR;

namespace SoccerManager.Application.Commands.PlayerRating.CreatePlayerRating;

/// <summary>
/// Represents a request to give a player a new current rating.
/// </summary>
public sealed record CreatePlayerRatingRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the player to rate.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The new rating, from 1 to 100; it becomes the player's current rating.
    /// </summary>
    public int Rating { get; init; }
}
