namespace SoccerManager.Application.Commands.Standing.UpdateStanding;

/// <summary>
/// Applies <see cref="UpdateStandingRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateStandingMapper
{
    /// <summary>
    /// Applies the request values to the given standing, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new standing values.</param>
    /// <param name="standing">The loaded standing entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateStandingRequest request, SoccerManager.Domain.Entities.Standing standing, DateTime now, Guid userId)
    {
        standing.SeasonId = request.SeasonId;
        standing.DivisionId = request.DivisionId;
        standing.TeamId = request.TeamId;
        standing.Points = request.Points;
        standing.GoalsFor = request.GoalsFor;
        standing.GoalsAgainst = request.GoalsAgainst;
        standing.Wins = request.Wins;
        standing.Draws = request.Draws;
        standing.Losses = request.Losses;
        standing.ModifiedAt = now;
        standing.ModifiedBy = userId;
    }
}
