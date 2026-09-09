using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Player.GetPlayerById;

/// <summary>
/// Maps player domain entities to <see cref="PlayerDto"/> instances.
/// </summary>
public static class GetPlayerByIdMapper
{
    /// <summary>
    /// Converts a player entity into its DTO representation.
    /// </summary>
    /// <param name="player">The player entity to convert.</param>
    /// <returns>The corresponding <see cref="PlayerDto"/>.</returns>
    public static PlayerDto ToDto(SoccerManager.Domain.Entities.Player player)
    {
        return new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            DateOfBirth = player.DateOfBirth,
            Rating = player.Rating,
            Value = player.Value,
            Wage = player.Wage,
            ImageUrl = player.ImageUrl,
            CreatedAt = player.CreatedAt,
            ModifiedAt = player.ModifiedAt,
        };
    }
}
