using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuries;

/// <summary>
/// Maps injury domain entities to <see cref="LeagueTeamPlayerInjuryDto"/> instances.
/// </summary>
public static class GetLeagueTeamPlayerInjuriesMapper
{
    /// <summary>
    /// Converts an injury entity into its DTO representation.
    /// </summary>
    /// <param name="leagueTeamPlayerInjury">The injury entity to convert.</param>
    /// <returns>The corresponding <see cref="LeagueTeamPlayerInjuryDto"/>.</returns>
    public static LeagueTeamPlayerInjuryDto ToDto(SoccerManager.Domain.Entities.LeagueTeamPlayerInjury leagueTeamPlayerInjury)
    {
        return new LeagueTeamPlayerInjuryDto
        {
            Id = leagueTeamPlayerInjury.Id,
            LeagueTeamPlayerId = leagueTeamPlayerInjury.LeagueTeamPlayerId,
            Name = leagueTeamPlayerInjury.Name,
            InjuryStartDate = leagueTeamPlayerInjury.InjuryStartDate,
            InjuryEndDate = leagueTeamPlayerInjury.InjuryEndDate,
            InjuryScale = leagueTeamPlayerInjury.InjuryScale,
            CreatedAt = leagueTeamPlayerInjury.CreatedAt,
            ModifiedAt = leagueTeamPlayerInjury.ModifiedAt,
        };
    }
}
