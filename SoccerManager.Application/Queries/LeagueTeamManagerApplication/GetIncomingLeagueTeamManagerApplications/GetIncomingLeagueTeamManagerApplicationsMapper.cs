using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetIncomingLeagueTeamManagerApplications;

/// <summary>
/// Maps <see cref="GetPendingLeagueTeamManagerApplicationsByLeagueOwnerResult"/> instances to <see cref="IncomingLeagueTeamManagerApplicationDto"/> instances.
/// </summary>
public static class GetIncomingLeagueTeamManagerApplicationsMapper
{
    /// <summary>
    /// Converts a materialized row into its DTO representation.
    /// </summary>
    /// <param name="row">The row to convert.</param>
    /// <returns>The corresponding <see cref="IncomingLeagueTeamManagerApplicationDto"/>.</returns>
    public static IncomingLeagueTeamManagerApplicationDto ToDto(GetPendingLeagueTeamManagerApplicationsByLeagueOwnerResult row)
    {
        return new IncomingLeagueTeamManagerApplicationDto
        {
            Id = row.Id,
            LeagueTeamId = row.LeagueTeamId,
            LeagueId = row.LeagueId,
            LeagueName = row.LeagueName,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            ManagerId = row.ManagerId,
            ManagerName = row.ManagerName,
            ManagerImageUrl = row.ManagerImageUrl,
            CreatedAt = row.CreatedAt,
        };
    }
}
