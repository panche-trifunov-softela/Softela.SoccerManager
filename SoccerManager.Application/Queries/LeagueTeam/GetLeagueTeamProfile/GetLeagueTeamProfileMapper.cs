using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamProfile;

/// <summary>
/// Maps a league team's profile row to <see cref="LeagueTeamProfileDto"/> instances.
/// </summary>
public static class GetLeagueTeamProfileMapper
{
    /// <summary>
    /// Converts a profile row into its DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeagueTeamProfileDto"/>.</returns>
    public static LeagueTeamProfileDto ToDto(GetLeagueTeamProfileResult row)
    {
        return new LeagueTeamProfileDto
        {
            LeagueTeamId = row.LeagueTeamId,
            LeagueId = row.LeagueId,
            LeagueName = row.LeagueName,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
        };
    }
}
