using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.PlayerRating.GetPlayerRatingById;

/// <summary>
/// Maps player rating domain entities to <see cref="PlayerRatingDto"/> instances.
/// </summary>
public static class GetPlayerRatingByIdMapper
{
    /// <summary>
    /// Converts a player rating entity into its DTO representation.
    /// </summary>
    /// <param name="playerRating">The player rating entity to convert.</param>
    /// <returns>The corresponding <see cref="PlayerRatingDto"/>.</returns>
    public static PlayerRatingDto ToDto(SoccerManager.Domain.Entities.PlayerRating playerRating)
    {
        return new PlayerRatingDto
        {
            Id = playerRating.Id,
            PlayerId = playerRating.PlayerId,
            Rating = playerRating.Rating,
            IsCurrent = playerRating.IsCurrent,
            CreatedAt = playerRating.CreatedAt,
            ModifiedAt = playerRating.ModifiedAt,
        };
    }
}
