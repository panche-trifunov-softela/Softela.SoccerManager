using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Division.GetDivisions;

/// <summary>
/// Maps division domain entities to <see cref="DivisionDto"/> instances.
/// </summary>
public static class GetDivisionsMapper
{
    /// <summary>
    /// Converts a division entity into its DTO representation.
    /// </summary>
    /// <param name="division">The division entity to convert.</param>
    /// <returns>The corresponding <see cref="DivisionDto"/>.</returns>
    public static DivisionDto ToDto(SoccerManager.Domain.Entities.Division division)
    {
        return new DivisionDto
        {
            Id = division.Id,
            LeagueId = division.LeagueId,
            Name = division.Name,
            Order = division.Order,
            TeamsPromoted = division.TeamsPromoted,
            TeamsRelegated = division.TeamsRelegated,
            TeamsInPlayoffs = division.TeamsInPlayoffs,
            CreatedAt = division.CreatedAt,
            ModifiedAt = division.ModifiedAt,
        };
    }
}
