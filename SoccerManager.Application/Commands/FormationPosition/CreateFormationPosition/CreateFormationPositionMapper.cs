namespace SoccerManager.Application.Commands.FormationPosition.CreateFormationPosition;

/// <summary>
/// Maps <see cref="CreateFormationPositionRequest"/> instances to domain entities.
/// </summary>
public static class CreateFormationPositionMapper
{
    /// <summary>
    /// Creates a new formation position entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the formation position values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the formation position.</param>
    /// <returns>A new, unsaved formation position entity.</returns>
    public static SoccerManager.Domain.Entities.FormationPosition ToDomainEntity(CreateFormationPositionRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.FormationPosition
        {
            FormationId = request.FormationId,
            PositionId = request.PositionId,
            SlotNumber = request.SlotNumber,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
