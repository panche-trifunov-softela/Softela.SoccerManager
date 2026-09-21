namespace SoccerManager.Application.Commands.Formation.CreateFormation;

/// <summary>
/// Maps <see cref="CreateFormationRequest"/> instances to domain entities.
/// </summary>
public static class CreateFormationMapper
{
    /// <summary>
    /// Creates a new formation entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the formation values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the formation.</param>
    /// <returns>A new, unsaved formation entity.</returns>
    public static SoccerManager.Domain.Entities.Formation ToDomainEntity(CreateFormationRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Formation
        {
            Name = request.Name,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
