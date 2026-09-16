using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagers;

/// <summary>
/// Maps league team manager domain entities to <see cref="LeagueTeamManagerDto"/> instances.
/// </summary>
public static class GetLeagueTeamManagersMapper
{
    /// <summary>
    /// Converts a league team manager entity into its DTO representation.
    /// </summary>
    /// <param name="leagueTeamManager">The league team manager entity to convert.</param>
    /// <returns>The corresponding <see cref="LeagueTeamManagerDto"/>.</returns>
    public static LeagueTeamManagerDto ToDto(SoccerManager.Domain.Entities.LeagueTeamManager leagueTeamManager)
    {
        return new LeagueTeamManagerDto
        {
            Id = leagueTeamManager.Id,
            LeagueId = leagueTeamManager.LeagueId,
            TeamId = leagueTeamManager.TeamId,
            ManagerId = leagueTeamManager.ManagerId,
            StartDate = leagueTeamManager.StartDate,
            EndDate = leagueTeamManager.EndDate,
            IsCurrent = leagueTeamManager.IsCurrent,
            CreatedAt = leagueTeamManager.CreatedAt,
            ModifiedAt = leagueTeamManager.ModifiedAt,
        };
    }
}
