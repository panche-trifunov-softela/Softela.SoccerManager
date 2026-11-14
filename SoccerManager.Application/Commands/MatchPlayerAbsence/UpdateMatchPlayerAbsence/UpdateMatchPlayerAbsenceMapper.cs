namespace SoccerManager.Application.Commands.MatchPlayerAbsence.UpdateMatchPlayerAbsence;

/// <summary>
/// Applies <see cref="UpdateMatchPlayerAbsenceRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateMatchPlayerAbsenceMapper
{
    /// <summary>
    /// Applies the request values to the given match player absence, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new match player absence values.</param>
    /// <param name="matchPlayerAbsence">The loaded match player absence entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateMatchPlayerAbsenceRequest request, SoccerManager.Domain.Entities.MatchPlayerAbsence matchPlayerAbsence, DateTime now, Guid userId)
    {
        matchPlayerAbsence.MatchId = request.MatchId;
        matchPlayerAbsence.LeagueTeamId = request.LeagueTeamId;
        matchPlayerAbsence.PlayerId = request.PlayerId;
        matchPlayerAbsence.Reason = request.Reason;
        matchPlayerAbsence.ModifiedAt = now;
        matchPlayerAbsence.ModifiedBy = userId;
    }
}
