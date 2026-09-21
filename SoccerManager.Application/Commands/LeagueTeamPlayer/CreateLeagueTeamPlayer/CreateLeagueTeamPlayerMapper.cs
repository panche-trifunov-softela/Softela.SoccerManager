namespace SoccerManager.Application.Commands.LeagueTeamPlayer.CreateLeagueTeamPlayer;

/// <summary>
/// Maps <see cref="CreateLeagueTeamPlayerRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueTeamPlayerMapper
{
    /// <summary>
    /// Creates a new league team player entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the league team player values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the league team player.</param>
    /// <returns>A new, unsaved league team player entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamPlayer ToDomainEntity(CreateLeagueTeamPlayerRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamPlayer
        {
            LeagueId = request.LeagueId,
            TeamId = request.TeamId,
            PlayerId = request.PlayerId,
            ContractLength = request.ContractLength,
            ContractSalaryPerWeek = request.ContractSalaryPerWeek,
            SquadNumber = request.SquadNumber,
            FansFavoritePlayer = request.FansFavoritePlayer,
            Morale = request.Morale,
            TransfermarketValue = request.TransfermarketValue,
            WantedStarterAppearances = request.WantedStarterAppearances,
            WantedTotalAppearances = request.WantedTotalAppearances,
            Condition = request.Condition,
            IsSuspendedDomesticCompetition = request.IsSuspendedDomesticCompetition,
            IsSuspendedContinentalCompetition = request.IsSuspendedContinentalCompetition,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
