namespace SoccerManager.Application.Commands.LeagueTeamManager.CreateLeagueTeamManager;

/// <summary>
/// Maps <see cref="CreateLeagueTeamManagerRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueTeamManagerMapper
{
    /// <summary>
    /// Creates a new league team manager entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the league team manager values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the league team manager.</param>
    /// <returns>A new, unsaved league team manager entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamManager ToDomainEntity(CreateLeagueTeamManagerRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamManager
        {
            LeagueId = request.LeagueId,
            TeamId = request.TeamId,
            ManagerId = request.ManagerId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsCurrent = request.IsCurrent,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
