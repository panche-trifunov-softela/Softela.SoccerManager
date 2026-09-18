namespace SoccerManager.Application.Commands.Match.UpdateMatch;

/// <summary>
/// Applies <see cref="UpdateMatchRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateMatchMapper
{
    /// <summary>
    /// Applies the request values to the given match, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new match values.</param>
    /// <param name="match">The loaded match entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateMatchRequest request, SoccerManager.Domain.Entities.Match match, DateTime now, Guid userId)
    {
        match.SeasonId = request.SeasonId;
        match.DivisionId = request.DivisionId;
        match.HomeTeamId = request.HomeTeamId;
        match.AwayTeamId = request.AwayTeamId;
        match.RefereeId = request.RefereeId;
        match.StartDateTime = request.StartDateTime;
        match.Commentary = request.Commentary;
        match.Attendance = request.Attendance;
        match.ModifiedAt = now;
        match.ModifiedBy = userId;
    }
}
