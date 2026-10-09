using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.CreateLeagueTeamManagerApplication;

/// <summary>
/// Maps <see cref="CreateLeagueTeamManagerApplicationRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueTeamManagerApplicationMapper
{
    /// <summary>
    /// Creates a new, pending league team manager application from the given request.
    /// </summary>
    /// <param name="request">The request carrying the league and team values.</param>
    /// <param name="managerId">The identifier of the applying manager, taken from the caller's manager profile.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the application.</param>
    /// <returns>A new, unsaved league team manager application entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamManagerApplication ToDomainEntity(CreateLeagueTeamManagerApplicationRequest request, int managerId, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamManagerApplication
        {
            LeagueId = request.LeagueId,
            TeamId = request.TeamId,
            ManagerId = managerId,
            Status = ApplicationStatus.Pending,
            ResponseDate = null,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
