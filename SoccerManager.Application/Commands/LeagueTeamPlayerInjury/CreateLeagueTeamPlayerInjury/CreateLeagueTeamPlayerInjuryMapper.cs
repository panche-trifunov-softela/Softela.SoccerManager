namespace SoccerManager.Application.Commands.LeagueTeamPlayerInjury.CreateLeagueTeamPlayerInjury;

/// <summary>
/// Maps <see cref="CreateLeagueTeamPlayerInjuryRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueTeamPlayerInjuryMapper
{
    /// <summary>
    /// Creates a new injury entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the injury values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the injury.</param>
    /// <returns>A new, unsaved injury entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamPlayerInjury ToDomainEntity(CreateLeagueTeamPlayerInjuryRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamPlayerInjury
        {
            LeagueTeamPlayerId = request.LeagueTeamPlayerId,
            Name = request.Name,
            InjuryStartDate = request.InjuryStartDate,
            InjuryEndDate = request.InjuryEndDate,
            InjuryScale = request.InjuryScale,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
