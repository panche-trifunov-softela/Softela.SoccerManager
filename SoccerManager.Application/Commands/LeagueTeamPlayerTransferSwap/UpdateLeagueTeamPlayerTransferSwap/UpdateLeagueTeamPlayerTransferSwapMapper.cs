namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.UpdateLeagueTeamPlayerTransferSwap;

/// <summary>
/// Applies <see cref="UpdateLeagueTeamPlayerTransferSwapRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateLeagueTeamPlayerTransferSwapMapper
{
    /// <summary>
    /// Applies the request values to the given swap, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new swap values.</param>
    /// <param name="leagueTeamPlayerTransferSwap">The loaded swap entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateLeagueTeamPlayerTransferSwapRequest request, SoccerManager.Domain.Entities.LeagueTeamPlayerTransferSwap leagueTeamPlayerTransferSwap, DateTime now, Guid userId)
    {
        leagueTeamPlayerTransferSwap.LeagueTeamPlayerTransferId = request.LeagueTeamPlayerTransferId;
        leagueTeamPlayerTransferSwap.LeagueTeamPlayerId = request.LeagueTeamPlayerId;
        leagueTeamPlayerTransferSwap.ModifiedAt = now;
        leagueTeamPlayerTransferSwap.ModifiedBy = userId;
    }
}
