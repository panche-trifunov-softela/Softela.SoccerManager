namespace SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayer;

/// <summary>
/// Applies <see cref="UpdateLeagueTeamPlayerRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateLeagueTeamPlayerMapper
{
    /// <summary>
    /// Applies the request values to the given league team player, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new league team player values.</param>
    /// <param name="leagueTeamPlayer">The loaded league team player entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateLeagueTeamPlayerRequest request, SoccerManager.Domain.Entities.LeagueTeamPlayer leagueTeamPlayer, DateTime now, Guid userId)
    {
        leagueTeamPlayer.LeagueId = request.LeagueId;
        leagueTeamPlayer.TeamId = request.TeamId;
        leagueTeamPlayer.PlayerId = request.PlayerId;
        leagueTeamPlayer.ContractLength = request.ContractLength;
        leagueTeamPlayer.ContractSalaryPerWeek = request.ContractSalaryPerWeek;
        leagueTeamPlayer.SquadNumber = request.SquadNumber;
        leagueTeamPlayer.FansFavoritePlayer = request.FansFavoritePlayer;
        leagueTeamPlayer.Morale = request.Morale;
        leagueTeamPlayer.TransfermarketValue = request.TransfermarketValue;
        leagueTeamPlayer.WantedStarterAppearances = request.WantedStarterAppearances;
        leagueTeamPlayer.WantedTotalAppearances = request.WantedTotalAppearances;
        leagueTeamPlayer.Condition = request.Condition;
        leagueTeamPlayer.IsSuspendedDomesticCompetition = request.IsSuspendedDomesticCompetition;
        leagueTeamPlayer.IsSuspendedContinentalCompetition = request.IsSuspendedContinentalCompetition;
        leagueTeamPlayer.ModifiedAt = now;
        leagueTeamPlayer.ModifiedBy = userId;
    }
}
