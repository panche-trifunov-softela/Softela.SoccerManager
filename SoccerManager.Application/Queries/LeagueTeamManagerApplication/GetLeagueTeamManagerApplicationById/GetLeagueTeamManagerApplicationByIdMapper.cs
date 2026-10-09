using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplicationById;

/// <summary>
/// Maps league team manager application domain entities to <see cref="LeagueTeamManagerApplicationDto"/> instances.
/// </summary>
public static class GetLeagueTeamManagerApplicationByIdMapper
{
    /// <summary>
    /// Converts a league team manager application entity into its DTO representation.
    /// </summary>
    /// <param name="application">The league team manager application entity to convert.</param>
    /// <returns>The corresponding <see cref="LeagueTeamManagerApplicationDto"/>.</returns>
    public static LeagueTeamManagerApplicationDto ToDto(SoccerManager.Domain.Entities.LeagueTeamManagerApplication application)
    {
        return new LeagueTeamManagerApplicationDto
        {
            Id = application.Id,
            LeagueId = application.LeagueId,
            TeamId = application.TeamId,
            ManagerId = application.ManagerId,
            Status = application.Status,
            ResponseDate = application.ResponseDate,
            CreatedAt = application.CreatedAt,
            ModifiedAt = application.ModifiedAt,
        };
    }
}
