namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.UpdateLeagueTeamPlayerTransfer;

/// <summary>
/// Applies <see cref="UpdateLeagueTeamPlayerTransferRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateLeagueTeamPlayerTransferMapper
{
    /// <summary>
    /// Applies the request values to the given transfer, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new transfer values.</param>
    /// <param name="leagueTeamPlayerTransfer">The loaded transfer entity to mutate.</param>
    /// <param name="fromLeagueTeamId">The identifier of the selling club: the stored one, or the club of the new squad row when the transfer moves to another one.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateLeagueTeamPlayerTransferRequest request, SoccerManager.Domain.Entities.LeagueTeamPlayerTransfer leagueTeamPlayerTransfer, int fromLeagueTeamId, DateTime now, Guid userId)
    {
        leagueTeamPlayerTransfer.LeagueTeamPlayerId = request.LeagueTeamPlayerId;
        leagueTeamPlayerTransfer.FromLeagueTeamId = fromLeagueTeamId;
        leagueTeamPlayerTransfer.ToLeagueTeamId = request.ToLeagueTeamId;
        leagueTeamPlayerTransfer.SeasonId = request.SeasonId;
        leagueTeamPlayerTransfer.TransferFee = request.TransferFee;
        leagueTeamPlayerTransfer.BuybackValue = request.BuybackValue;
        leagueTeamPlayerTransfer.BuybackValidSeasons = request.BuybackValidSeasons;
        leagueTeamPlayerTransfer.Status = request.Status;
        leagueTeamPlayerTransfer.ModifiedAt = now;
        leagueTeamPlayerTransfer.ModifiedBy = userId;
    }
}
