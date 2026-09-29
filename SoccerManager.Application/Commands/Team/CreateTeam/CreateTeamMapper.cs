namespace SoccerManager.Application.Commands.Team.CreateTeam;

/// <summary>
/// Maps <see cref="CreateTeamRequest"/> instances to domain entities.
/// </summary>
public static class CreateTeamMapper
{
    /// <summary>
    /// Creates a new team entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the team values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the team.</param>
    /// <returns>A new, unsaved team entity.</returns>
    public static SoccerManager.Domain.Entities.Team ToDomainEntity(CreateTeamRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Team
        {
            Name = request.Name,
            StadiumId = request.StadiumId,
            FinancialState = request.FinancialState,
            JerseyUrl = request.JerseyUrl,
            LogoUrl = request.LogoUrl,
            TransfermarktId = request.TransfermarktId,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
