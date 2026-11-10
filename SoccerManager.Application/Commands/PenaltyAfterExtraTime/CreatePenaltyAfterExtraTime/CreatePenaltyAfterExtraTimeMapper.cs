namespace SoccerManager.Application.Commands.PenaltyAfterExtraTime.CreatePenaltyAfterExtraTime;

/// <summary>
/// Maps <see cref="CreatePenaltyAfterExtraTimeRequest"/> instances to domain entities.
/// </summary>
public static class CreatePenaltyAfterExtraTimeMapper
{
    /// <summary>
    /// Creates a new penalty after extra time entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the penalty after extra time values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the penalty after extra time.</param>
    /// <returns>A new, unsaved penalty after extra time entity.</returns>
    public static SoccerManager.Domain.Entities.PenaltyAfterExtraTime ToDomainEntity(CreatePenaltyAfterExtraTimeRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.PenaltyAfterExtraTime
        {
            MatchId = request.MatchId,
            KickNumber = request.KickNumber,
            LeagueTeamId = request.LeagueTeamId,
            PlayerId = request.PlayerId,
            IsScored = request.IsScored,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
