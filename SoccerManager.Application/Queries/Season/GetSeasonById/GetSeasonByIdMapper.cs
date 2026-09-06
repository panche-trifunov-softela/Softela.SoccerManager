using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Season.GetSeasonById;

/// <summary>
/// Maps season domain entities to <see cref="SeasonDto"/> instances.
/// </summary>
public static class GetSeasonByIdMapper
{
    /// <summary>
    /// Converts a season entity into its DTO representation.
    /// </summary>
    /// <param name="season">The season entity to convert.</param>
    /// <returns>The corresponding <see cref="SeasonDto"/>.</returns>
    public static SeasonDto ToDto(SoccerManager.Domain.Entities.Season season)
    {
        return new SeasonDto
        {
            Id = season.Id,
            LeagueId = season.LeagueId,
            SeasonNumber = season.SeasonNumber,
            CreatedAt = season.CreatedAt,
            ModifiedAt = season.ModifiedAt,
        };
    }
}
