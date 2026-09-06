namespace SoccerManager.Application.Commands.League.UpdateLeague;

/// <summary>
/// Applies <see cref="UpdateLeagueRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateLeagueMapper
{
    /// <summary>
    /// Applies the request values to the given league, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new league values.</param>
    /// <param name="league">The loaded league entity to mutate.</param>
    /// <param name="now">The timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateLeagueRequest request, SoccerManager.Domain.Entities.League league, DateTimeOffset now, Guid userId)
    {
        league.Name = request.Name;
        league.ModifiedAt = now;
        league.ModifiedBy = userId;
    }
}
