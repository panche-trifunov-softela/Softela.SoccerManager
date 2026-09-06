using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.League.GetLeagues;

/// <summary>
/// Maps league domain entities to <see cref="LeagueDto"/> instances.
/// </summary>
public static class GetLeaguesMapper
{
    /// <summary>
    /// Converts a league entity into its DTO representation.
    /// </summary>
    /// <param name="league">The league entity to convert.</param>
    /// <returns>The corresponding <see cref="LeagueDto"/>.</returns>
    public static LeagueDto ToDto(SoccerManager.Domain.Entities.League league)
    {
        return new LeagueDto
        {
            Id = league.Id,
            Name = league.Name,
            CreatedAt = league.CreatedAt,
            ModifiedAt = league.ModifiedAt,
        };
    }
}
