using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.PlayerRating.GetPlayerRatingById;

/// <summary>
/// Represents the result of a <see cref="GetPlayerRatingByIdRequest"/> query.
/// </summary>
public sealed record GetPlayerRatingByIdResponse
{
    /// <summary>
    /// The requested player rating.
    /// </summary>
    public required PlayerRatingDto Data { get; init; }
}
