using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;

/// <summary>
/// Maps available-club rows to <see cref="AvailableLeagueTeamDto"/> instances.
/// </summary>
public static class GetAvailableLeagueTeamsMapper
{
    /// <summary>
    /// Converts an available-club row into its DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="AvailableLeagueTeamDto"/>.</returns>
    public static AvailableLeagueTeamDto ToDto(GetAvailableLeagueTeamsResult row)
    {
        return new AvailableLeagueTeamDto
        {
            Id = row.Id,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            TeamFinancialState = row.TeamFinancialState,
            IsManaged = row.IsManaged,
        };
    }
}
