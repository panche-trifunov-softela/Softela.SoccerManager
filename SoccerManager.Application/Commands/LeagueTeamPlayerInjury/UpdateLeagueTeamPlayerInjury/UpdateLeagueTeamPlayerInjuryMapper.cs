namespace SoccerManager.Application.Commands.LeagueTeamPlayerInjury.UpdateLeagueTeamPlayerInjury;

/// <summary>
/// Applies <see cref="UpdateLeagueTeamPlayerInjuryRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateLeagueTeamPlayerInjuryMapper
{
    /// <summary>
    /// Applies the request values to the given injury, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new injury values.</param>
    /// <param name="leagueTeamPlayerInjury">The loaded injury entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateLeagueTeamPlayerInjuryRequest request, SoccerManager.Domain.Entities.LeagueTeamPlayerInjury leagueTeamPlayerInjury, DateTime now, Guid userId)
    {
        leagueTeamPlayerInjury.LeagueTeamPlayerId = request.LeagueTeamPlayerId;
        leagueTeamPlayerInjury.Name = request.Name;
        leagueTeamPlayerInjury.InjuryStartDate = request.InjuryStartDate;
        leagueTeamPlayerInjury.InjuryEndDate = request.InjuryEndDate;
        leagueTeamPlayerInjury.InjuryScale = request.InjuryScale;
        leagueTeamPlayerInjury.ModifiedAt = now;
        leagueTeamPlayerInjury.ModifiedBy = userId;
    }
}
