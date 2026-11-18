namespace SoccerManager.Application.Commands.NationalTeamMatch.UpdateNationalTeamMatch;

/// <summary>
/// Applies <see cref="UpdateNationalTeamMatchRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateNationalTeamMatchMapper
{
    /// <summary>
    /// Applies the request values to the given national team match, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new national team match values.</param>
    /// <param name="nationalTeamMatch">The loaded national team match entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateNationalTeamMatchRequest request, SoccerManager.Domain.Entities.NationalTeamMatch nationalTeamMatch, DateTime now, Guid userId)
    {
        nationalTeamMatch.SeasonId = request.SeasonId;
        nationalTeamMatch.HomeNationalTeamId = request.HomeNationalTeamId;
        nationalTeamMatch.AwayNationalTeamId = request.AwayNationalTeamId;
        nationalTeamMatch.StartDateTime = request.StartDateTime;
        nationalTeamMatch.Stage = request.Stage;
        nationalTeamMatch.GroupNumber = request.GroupNumber;
        nationalTeamMatch.RoundNumber = request.RoundNumber;
        nationalTeamMatch.Leg = request.Leg;
        nationalTeamMatch.IsStarted = request.IsStarted;
        nationalTeamMatch.IsFinished = request.IsFinished;
        nationalTeamMatch.ModifiedAt = now;
        nationalTeamMatch.ModifiedBy = userId;
    }
}
