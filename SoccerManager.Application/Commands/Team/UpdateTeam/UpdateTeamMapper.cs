namespace SoccerManager.Application.Commands.Team.UpdateTeam;

/// <summary>
/// Applies <see cref="UpdateTeamRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateTeamMapper
{
    /// <summary>
    /// Applies the request values to the given team, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new team values.</param>
    /// <param name="team">The loaded team entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateTeamRequest request, SoccerManager.Domain.Entities.Team team, DateTime now, Guid userId)
    {
        team.Name = request.Name;
        team.StadiumId = request.StadiumId;
        team.FinancialState = request.FinancialState;
        team.JerseyUrl = request.JerseyUrl;
        team.LogoUrl = request.LogoUrl;
        team.ModifiedAt = now;
        team.ModifiedBy = userId;
    }
}
