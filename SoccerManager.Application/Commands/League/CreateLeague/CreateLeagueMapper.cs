namespace SoccerManager.Application.Commands.League.CreateLeague;

/// <summary>
/// Maps <see cref="CreateLeagueRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueMapper
{
    /// <summary>
    /// Creates a new league entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the league name.</param>
    /// <param name="ownerManagerId">The identifier of the creating user's manager profile, which owns the league.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the league.</param>
    /// <returns>A new, unsaved league entity.</returns>
    public static SoccerManager.Domain.Entities.League ToDomainEntity(CreateLeagueRequest request, int ownerManagerId, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.League
        {
            Name = request.Name,
            OwnerManagerId = ownerManagerId,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }

    /// <summary>
    /// Creates the roster entry of a division club of a new league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league the club takes part in.</param>
    /// <param name="teamId">The identifier of the team that is the division club.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the league.</param>
    /// <returns>A new, unsaved league team entity that is not a guest.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeam ToDivisionLeagueTeam(int leagueId, int teamId, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeam
        {
            LeagueId = leagueId,
            TeamId = teamId,
            IsGuest = false,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
