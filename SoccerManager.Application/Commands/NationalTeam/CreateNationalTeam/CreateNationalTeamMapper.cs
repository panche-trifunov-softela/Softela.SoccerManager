namespace SoccerManager.Application.Commands.NationalTeam.CreateNationalTeam;

/// <summary>
/// Maps <see cref="CreateNationalTeamRequest"/> instances to domain entities.
/// </summary>
public static class CreateNationalTeamMapper
{
    /// <summary>
    /// Creates a new national team entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the national team values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the national team.</param>
    /// <returns>A new, unsaved national team entity.</returns>
    public static SoccerManager.Domain.Entities.NationalTeam ToDomainEntity(CreateNationalTeamRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.NationalTeam
        {
            Name = request.Name,
            StadiumId = request.StadiumId,
            JerseyUrl = request.JerseyUrl,
            LogoUrl = request.LogoUrl,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
