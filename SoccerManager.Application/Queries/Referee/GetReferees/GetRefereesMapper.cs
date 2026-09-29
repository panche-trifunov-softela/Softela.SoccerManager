using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Referee.GetReferees;

/// <summary>
/// Maps referee domain entities to <see cref="RefereeDto"/> instances.
/// </summary>
public static class GetRefereesMapper
{
    /// <summary>
    /// Converts a referee entity into its DTO representation.
    /// </summary>
    /// <param name="referee">The referee entity to convert.</param>
    /// <returns>The corresponding <see cref="RefereeDto"/>.</returns>
    public static RefereeDto ToDto(SoccerManager.Domain.Entities.Referee referee)
    {
        return new RefereeDto
        {
            Id = referee.Id,
            Name = referee.Name,
            ImageUrl = referee.ImageUrl,
            Tolerance = referee.Tolerance,
            CreatedAt = referee.CreatedAt,
            ModifiedAt = referee.ModifiedAt,
        };
    }
}
