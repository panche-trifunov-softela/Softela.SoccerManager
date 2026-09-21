using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.FormationPosition.GetFormationPositions;

/// <summary>
/// Maps formation position domain entities to <see cref="FormationPositionDto"/> instances.
/// </summary>
public static class GetFormationPositionsMapper
{
    /// <summary>
    /// Converts a formation position entity into its DTO representation.
    /// </summary>
    /// <param name="formationPosition">The formation position entity to convert.</param>
    /// <returns>The corresponding <see cref="FormationPositionDto"/>.</returns>
    public static FormationPositionDto ToDto(SoccerManager.Domain.Entities.FormationPosition formationPosition)
    {
        return new FormationPositionDto
        {
            Id = formationPosition.Id,
            FormationId = formationPosition.FormationId,
            PositionId = formationPosition.PositionId,
            SlotNumber = formationPosition.SlotNumber,
            CreatedAt = formationPosition.CreatedAt,
            ModifiedAt = formationPosition.ModifiedAt,
        };
    }
}
