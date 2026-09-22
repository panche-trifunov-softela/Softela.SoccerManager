using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetMyLeagueTeamManagers;

/// <summary>
/// Maps <see cref="GetLeagueTeamManagersByUserIdResult"/> instances to <see cref="MyLeagueTeamManagerDto"/> instances.
/// </summary>
public static class GetMyLeagueTeamManagersMapper
{
    /// <summary>
    /// Converts a materialized row into its DTO representation.
    /// </summary>
    /// <param name="row">The row to convert.</param>
    /// <returns>The corresponding <see cref="MyLeagueTeamManagerDto"/>.</returns>
    public static MyLeagueTeamManagerDto ToDto(GetLeagueTeamManagersByUserIdResult row)
    {
        return new MyLeagueTeamManagerDto
        {
            Id = row.Id,
            LeagueId = row.LeagueId,
            LeagueName = row.LeagueName,
            TeamId = row.TeamId,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            ManagerId = row.ManagerId,
            StartDate = row.StartDate,
            EndDate = row.EndDate,
            IsCurrent = row.IsCurrent,
            CreatedAt = row.CreatedAt,
            ModifiedAt = row.ModifiedAt,
        };
    }
}
