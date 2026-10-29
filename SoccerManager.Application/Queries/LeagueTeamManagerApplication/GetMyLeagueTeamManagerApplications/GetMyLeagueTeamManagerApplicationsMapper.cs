using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetMyLeagueTeamManagerApplications;

/// <summary>
/// Maps <see cref="GetPendingLeagueTeamManagerApplicationsByUserIdResult"/> instances to <see cref="MyLeagueTeamManagerApplicationDto"/> instances.
/// </summary>
public static class GetMyLeagueTeamManagerApplicationsMapper
{
    /// <summary>
    /// Converts a materialized row into its DTO representation.
    /// </summary>
    /// <param name="row">The row to convert.</param>
    /// <returns>The corresponding <see cref="MyLeagueTeamManagerApplicationDto"/>.</returns>
    public static MyLeagueTeamManagerApplicationDto ToDto(GetPendingLeagueTeamManagerApplicationsByUserIdResult row)
    {
        return new MyLeagueTeamManagerApplicationDto
        {
            Id = row.Id,
            LeagueTeamId = row.LeagueTeamId,
            LeagueId = row.LeagueId,
            LeagueName = row.LeagueName,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            CreatedAt = row.CreatedAt,
        };
    }
}
