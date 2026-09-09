namespace SoccerManager.Application.Commands.Division.UpdateDivision;

/// <summary>
/// Applies <see cref="UpdateDivisionRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateDivisionMapper
{
    /// <summary>
    /// Applies the request values to the given division, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new division values.</param>
    /// <param name="division">The loaded division entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateDivisionRequest request, SoccerManager.Domain.Entities.Division division, DateTime now, Guid userId)
    {
        // LeagueId is intentionally left untouched: it is fixed at creation, so a division never moves between leagues.
        division.Name = request.Name;
        division.Order = request.Order;
        division.TeamsPromoted = request.TeamsPromoted;
        division.TeamsRelegated = request.TeamsRelegated;
        division.TeamsInPlayoffs = request.TeamsInPlayoffs;
        division.ModifiedAt = now;
        division.ModifiedBy = userId;
    }
}
