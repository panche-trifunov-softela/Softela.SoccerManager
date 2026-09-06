namespace SoccerManager.Application.Commands.Season.UpdateSeason;

/// <summary>
/// Applies <see cref="UpdateSeasonRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateSeasonMapper
{
    /// <summary>
    /// Applies the request values to the given season, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new season number.</param>
    /// <param name="season">The loaded season entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateSeasonRequest request, SoccerManager.Domain.Entities.Season season, DateTime now, Guid userId)
    {
        // LeagueId is intentionally left untouched: it is fixed at creation, so a season never moves between leagues.
        season.SeasonNumber = request.SeasonNumber;
        season.ModifiedAt = now;
        season.ModifiedBy = userId;
    }
}
