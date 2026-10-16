using MediatR;

namespace SoccerManager.Application.Commands.PlayerRating.DeletePlayerRating;

/// <summary>
/// Represents a request to delete a past player rating.
/// </summary>
public sealed record DeletePlayerRatingRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the player rating to delete.
    /// </summary>
    public int Id { get; init; }
}
