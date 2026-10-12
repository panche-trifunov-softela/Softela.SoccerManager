namespace SoccerManager.Application.Commands.LeagueTeam.CreateLeagueTeam;

/// <summary>
/// Maps <see cref="CreateLeagueTeamRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueTeamMapper
{
    /// <summary>
    /// Creates a new league team entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the league team values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the league team.</param>
    /// <returns>A new, unsaved league team entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeam ToDomainEntity(CreateLeagueTeamRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeam
        {
            LeagueId = request.LeagueId,
            TeamId = request.TeamId,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
