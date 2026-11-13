namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.CreateLeagueTeamPlayerTransfer;

/// <summary>
/// Maps <see cref="CreateLeagueTeamPlayerTransferRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueTeamPlayerTransferMapper
{
    /// <summary>
    /// Creates a new transfer entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the transfer values.</param>
    /// <param name="fromLeagueTeamId">The identifier of the selling club, taken from the transferred squad row.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the transfer.</param>
    /// <returns>A new, unsaved transfer entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamPlayerTransfer ToDomainEntity(CreateLeagueTeamPlayerTransferRequest request, int fromLeagueTeamId, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamPlayerTransfer
        {
            LeagueTeamPlayerId = request.LeagueTeamPlayerId,
            FromLeagueTeamId = fromLeagueTeamId,
            ToLeagueTeamId = request.ToLeagueTeamId,
            SeasonId = request.SeasonId,
            TransferFee = request.TransferFee,
            BuybackValue = request.BuybackValue,
            BuybackValidSeasons = request.BuybackValidSeasons,
            Status = request.Status,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
