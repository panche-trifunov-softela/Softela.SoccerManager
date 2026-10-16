using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.PlayerRating.GetPlayerRatings;

/// <summary>
/// Represents the result of a <see cref="GetPlayerRatingsRequest"/> query.
/// </summary>
public sealed record GetPlayerRatingsResponse
{
    /// <summary>
    /// The ratings the requested player has had, newest first.
    /// </summary>
    public required List<PlayerRatingDto> Data { get; init; }
}
