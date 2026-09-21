using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayers;

/// <summary>
/// Maps league team player domain entities to <see cref="LeagueTeamPlayerDto"/> instances.
/// </summary>
public static class GetLeagueTeamPlayersMapper
{
    /// <summary>
    /// Converts a league team player entity into its DTO representation.
    /// </summary>
    /// <param name="leagueTeamPlayer">The league team player entity to convert.</param>
    /// <returns>The corresponding <see cref="LeagueTeamPlayerDto"/>.</returns>
    public static LeagueTeamPlayerDto ToDto(SoccerManager.Domain.Entities.LeagueTeamPlayer leagueTeamPlayer)
    {
        return new LeagueTeamPlayerDto
        {
            Id = leagueTeamPlayer.Id,
            LeagueId = leagueTeamPlayer.LeagueId,
            TeamId = leagueTeamPlayer.TeamId,
            PlayerId = leagueTeamPlayer.PlayerId,
            ContractLength = leagueTeamPlayer.ContractLength,
            ContractSalaryPerWeek = leagueTeamPlayer.ContractSalaryPerWeek,
            SquadNumber = leagueTeamPlayer.SquadNumber,
            FansFavoritePlayer = leagueTeamPlayer.FansFavoritePlayer,
            Morale = leagueTeamPlayer.Morale,
            TransfermarketValue = leagueTeamPlayer.TransfermarketValue,
            WantedStarterAppearances = leagueTeamPlayer.WantedStarterAppearances,
            WantedTotalAppearances = leagueTeamPlayer.WantedTotalAppearances,
            Condition = leagueTeamPlayer.Condition,
            IsSuspendedDomesticCompetition = leagueTeamPlayer.IsSuspendedDomesticCompetition,
            IsSuspendedContinentalCompetition = leagueTeamPlayer.IsSuspendedContinentalCompetition,
            CreatedAt = leagueTeamPlayer.CreatedAt,
            ModifiedAt = leagueTeamPlayer.ModifiedAt,
        };
    }
}
