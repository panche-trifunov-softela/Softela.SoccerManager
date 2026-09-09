namespace SoccerManager.Application.Commands.Position.CreatePosition;

/// <summary>
/// Maps <see cref="CreatePositionRequest"/> instances to domain entities.
/// </summary>
public static class CreatePositionMapper
{
    /// <summary>
    /// Creates a new position entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the position values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the position.</param>
    /// <returns>A new, unsaved position entity.</returns>
    public static SoccerManager.Domain.Entities.Position ToDomainEntity(CreatePositionRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Position
        {
            Name = request.Name,
            Area = request.Area,
            Side = request.Side,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
