using MediatR;

namespace SoccerManager.Application.Queries.PlayerRating.GetPlayerRatings;

/// <summary>
/// Represents a request to retrieve every rating a player has had.
/// </summary>
public sealed record GetPlayerRatingsRequest : IRequest<GetPlayerRatingsResponse>
{
    /// <summary>
    /// The identifier of the player whose ratings are being requested.
    /// </summary>
    public int PlayerId { get; init; }
}
