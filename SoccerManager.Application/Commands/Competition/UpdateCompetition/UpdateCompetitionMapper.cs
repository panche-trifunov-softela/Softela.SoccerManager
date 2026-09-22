namespace SoccerManager.Application.Commands.Competition.UpdateCompetition;

/// <summary>
/// Applies <see cref="UpdateCompetitionRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateCompetitionMapper
{
    /// <summary>
    /// Applies the request values to the given competition, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new competition values.</param>
    /// <param name="competition">The loaded competition entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateCompetitionRequest request, SoccerManager.Domain.Entities.Competition competition, DateTime now, Guid userId)
    {
        // LeagueId is intentionally left untouched: it is fixed at creation, so a competition never moves between leagues.
        competition.Name = request.Name;
        competition.LogoUrl = request.LogoUrl;
        competition.IsDomestic = request.IsDomestic;
        competition.Format = request.Format;
        competition.MaxAgeAllowed = request.MaxAgeAllowed;
        competition.ModifiedAt = now;
        competition.ModifiedBy = userId;
    }
}
