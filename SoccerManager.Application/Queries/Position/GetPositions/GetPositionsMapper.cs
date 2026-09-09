using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Position.GetPositions;

/// <summary>
/// Maps position domain entities to <see cref="PositionDto"/> instances.
/// </summary>
public static class GetPositionsMapper
{
    /// <summary>
    /// Converts a position entity into its DTO representation.
    /// </summary>
    /// <param name="position">The position entity to convert.</param>
    /// <returns>The corresponding <see cref="PositionDto"/>.</returns>
    public static PositionDto ToDto(SoccerManager.Domain.Entities.Position position)
    {
        return new PositionDto
        {
            Id = position.Id,
            Name = position.Name,
            Area = position.Area,
            Side = position.Side,
            CreatedAt = position.CreatedAt,
            ModifiedAt = position.ModifiedAt,
        };
    }
}
