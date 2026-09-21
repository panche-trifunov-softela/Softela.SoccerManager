using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Formation.GetFormations;

/// <summary>
/// Maps formation domain entities to <see cref="FormationDto"/> instances.
/// </summary>
public static class GetFormationsMapper
{
    /// <summary>
    /// Converts a formation entity into its DTO representation.
    /// </summary>
    /// <param name="formation">The formation entity to convert.</param>
    /// <returns>The corresponding <see cref="FormationDto"/>.</returns>
    public static FormationDto ToDto(SoccerManager.Domain.Entities.Formation formation)
    {
        return new FormationDto
        {
            Id = formation.Id,
            Name = formation.Name,
            CreatedAt = formation.CreatedAt,
            ModifiedAt = formation.ModifiedAt,
        };
    }
}
