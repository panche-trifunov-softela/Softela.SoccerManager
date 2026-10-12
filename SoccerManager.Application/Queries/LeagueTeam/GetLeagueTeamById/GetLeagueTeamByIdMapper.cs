using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamById;

/// <summary>
/// Maps league team domain entities to <see cref="LeagueTeamDto"/> instances.
/// </summary>
public static class GetLeagueTeamByIdMapper
{
    /// <summary>
    /// Converts a league team entity into its DTO representation.
    /// </summary>
    /// <param name="leagueTeam">The league team entity to convert.</param>
    /// <returns>The corresponding <see cref="LeagueTeamDto"/>.</returns>
    public static LeagueTeamDto ToDto(SoccerManager.Domain.Entities.LeagueTeam leagueTeam)
    {
        return new LeagueTeamDto
        {
            Id = leagueTeam.Id,
            LeagueId = leagueTeam.LeagueId,
            TeamId = leagueTeam.TeamId,
            CreatedAt = leagueTeam.CreatedAt,
            ModifiedAt = leagueTeam.ModifiedAt,
        };
    }
}
