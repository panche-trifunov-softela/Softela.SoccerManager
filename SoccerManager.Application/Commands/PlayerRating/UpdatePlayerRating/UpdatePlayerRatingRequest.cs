using MediatR;

namespace SoccerManager.Application.Commands.PlayerRating.UpdatePlayerRating;

/// <summary>
/// Represents a request to correct the value of an existing player rating.
/// </summary>
public sealed record UpdatePlayerRatingRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the player rating to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The corrected rating value, from 1 to 100.
    /// </summary>
    public int Rating { get; init; }
}
