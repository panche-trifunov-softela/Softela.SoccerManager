namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.CreateLeagueTeamPlayerTransferSwap;

/// <summary>
/// Maps <see cref="CreateLeagueTeamPlayerTransferSwapRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueTeamPlayerTransferSwapMapper
{
    /// <summary>
    /// Creates a new swap entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the swap values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the swap.</param>
    /// <returns>A new, unsaved swap entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamPlayerTransferSwap ToDomainEntity(CreateLeagueTeamPlayerTransferSwapRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamPlayerTransferSwap
        {
            LeagueTeamPlayerTransferId = request.LeagueTeamPlayerTransferId,
            LeagueTeamPlayerId = request.LeagueTeamPlayerId,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
