namespace SoccerManager.Application.Commands.MatchPlayerAbsence.CreateMatchPlayerAbsence;

/// <summary>
/// Maps <see cref="CreateMatchPlayerAbsenceRequest"/> instances to domain entities.
/// </summary>
public static class CreateMatchPlayerAbsenceMapper
{
    /// <summary>
    /// Creates a new match player absence entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the match player absence values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the match player absence.</param>
    /// <returns>A new, unsaved match player absence entity.</returns>
    public static SoccerManager.Domain.Entities.MatchPlayerAbsence ToDomainEntity(CreateMatchPlayerAbsenceRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.MatchPlayerAbsence
        {
            MatchId = request.MatchId,
            LeagueTeamId = request.LeagueTeamId,
            PlayerId = request.PlayerId,
            Reason = request.Reason,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
