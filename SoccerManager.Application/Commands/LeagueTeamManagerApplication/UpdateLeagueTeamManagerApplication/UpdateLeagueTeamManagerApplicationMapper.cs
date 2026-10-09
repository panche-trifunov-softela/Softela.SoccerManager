namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.UpdateLeagueTeamManagerApplication;

/// <summary>
/// Applies <see cref="UpdateLeagueTeamManagerApplicationRequest"/> values onto an existing domain entity, and builds the appointment an acceptance creates.
/// </summary>
public static class UpdateLeagueTeamManagerApplicationMapper
{
    /// <summary>
    /// Applies the answer to the given application, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the answer.</param>
    /// <param name="application">The loaded application entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the response date and the modified field.</param>
    /// <param name="userId">The identifier of the user answering the application.</param>
    public static void ApplyTo(UpdateLeagueTeamManagerApplicationRequest request, SoccerManager.Domain.Entities.LeagueTeamManagerApplication application, DateTime now, Guid userId)
    {
        application.Status = request.Status;
        application.ResponseDate = now;
        application.ModifiedAt = now;
        application.ModifiedBy = userId;
    }

    /// <summary>
    /// Builds the current manager appointment that accepting the given application creates.
    /// </summary>
    /// <param name="application">The accepted application the appointment is for.</param>
    /// <param name="now">The UTC timestamp whose date starts the tenure and which is stamped on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user accepting the application.</param>
    /// <returns>A new, unsaved league team manager appointment.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamManager ToAppointment(SoccerManager.Domain.Entities.LeagueTeamManagerApplication application, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamManager
        {
            LeagueId = application.LeagueId,
            TeamId = application.TeamId,
            ManagerId = application.ManagerId,
            StartDate = DateOnly.FromDateTime(now),
            EndDate = null,
            IsCurrent = true,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
