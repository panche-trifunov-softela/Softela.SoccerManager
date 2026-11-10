namespace SoccerManager.Application.Commands.PenaltyAfterExtraTime.UpdatePenaltyAfterExtraTime;

/// <summary>
/// Applies <see cref="UpdatePenaltyAfterExtraTimeRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdatePenaltyAfterExtraTimeMapper
{
    /// <summary>
    /// Applies the request values to the given penalty after extra time, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new penalty after extra time values.</param>
    /// <param name="penaltyAfterExtraTime">The loaded penalty after extra time entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdatePenaltyAfterExtraTimeRequest request, SoccerManager.Domain.Entities.PenaltyAfterExtraTime penaltyAfterExtraTime, DateTime now, Guid userId)
    {
        penaltyAfterExtraTime.MatchId = request.MatchId;
        penaltyAfterExtraTime.KickNumber = request.KickNumber;
        penaltyAfterExtraTime.LeagueTeamId = request.LeagueTeamId;
        penaltyAfterExtraTime.PlayerId = request.PlayerId;
        penaltyAfterExtraTime.IsScored = request.IsScored;
        penaltyAfterExtraTime.ModifiedAt = now;
        penaltyAfterExtraTime.ModifiedBy = userId;
    }
}
