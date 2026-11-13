using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerInjuries;

/// <summary>
/// Maps a player's injury rows to <see cref="PlayerInjuryDto"/> instances.
/// </summary>
public static class GetLeaguePlayerInjuriesMapper
{
    /// <summary>
    /// Converts an injury row into its DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="PlayerInjuryDto"/>.</returns>
    public static PlayerInjuryDto ToDto(GetLeaguePlayerInjuriesResult row)
    {
        return new PlayerInjuryDto
        {
            Id = row.Id,
            Name = row.Name,
            InjuryStartDate = row.InjuryStartDate,
            InjuryEndDate = row.InjuryEndDate,
            InjuryScale = row.InjuryScale,
        };
    }
}
