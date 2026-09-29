namespace SoccerManager.Application.Commands.NationalTeam.UpdateNationalTeam;

/// <summary>
/// Applies <see cref="UpdateNationalTeamRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateNationalTeamMapper
{
    /// <summary>
    /// Applies the request values to the given national team, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new national team values.</param>
    /// <param name="nationalTeam">The loaded national team entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateNationalTeamRequest request, SoccerManager.Domain.Entities.NationalTeam nationalTeam, DateTime now, Guid userId)
    {
        nationalTeam.Name = request.Name;
        nationalTeam.StadiumId = request.StadiumId;
        nationalTeam.JerseyUrl = request.JerseyUrl;
        nationalTeam.LogoUrl = request.LogoUrl;
        nationalTeam.TransfermarktId = request.TransfermarktId;
        nationalTeam.ModifiedAt = now;
        nationalTeam.ModifiedBy = userId;
    }
}
