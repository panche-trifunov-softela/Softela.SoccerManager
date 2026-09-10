using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositionById;

/// <summary>
/// Maps player position domain entities to <see cref="PlayerPositionDto"/> instances.
/// </summary>
public static class GetPlayerPositionByIdMapper
{
    /// <summary>
    /// Converts a player position entity into its DTO representation.
    /// </summary>
    /// <param name="playerPosition">The player position entity to convert.</param>
    /// <returns>The corresponding <see cref="PlayerPositionDto"/>.</returns>
    public static PlayerPositionDto ToDto(SoccerManager.Domain.Entities.PlayerPosition playerPosition)
    {
        return new PlayerPositionDto
        {
            Id = playerPosition.Id,
            PlayerId = playerPosition.PlayerId,
            PositionId = playerPosition.PositionId,
            Quality = playerPosition.Quality,
            CreatedAt = playerPosition.CreatedAt,
            ModifiedAt = playerPosition.ModifiedAt,
        };
    }
}
