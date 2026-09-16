namespace SoccerManager.Application.Commands.LeagueTeamManager.UpdateLeagueTeamManager;

/// <summary>
/// Applies <see cref="UpdateLeagueTeamManagerRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateLeagueTeamManagerMapper
{
    /// <summary>
    /// Applies the request values to the given league team manager, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new league team manager values.</param>
    /// <param name="leagueTeamManager">The loaded league team manager entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateLeagueTeamManagerRequest request, SoccerManager.Domain.Entities.LeagueTeamManager leagueTeamManager, DateTime now, Guid userId)
    {
        leagueTeamManager.LeagueId = request.LeagueId;
        leagueTeamManager.TeamId = request.TeamId;
        leagueTeamManager.ManagerId = request.ManagerId;
        leagueTeamManager.StartDate = request.StartDate;
        leagueTeamManager.EndDate = request.EndDate;
        leagueTeamManager.IsCurrent = request.IsCurrent;
        leagueTeamManager.ModifiedAt = now;
        leagueTeamManager.ModifiedBy = userId;
    }
}
