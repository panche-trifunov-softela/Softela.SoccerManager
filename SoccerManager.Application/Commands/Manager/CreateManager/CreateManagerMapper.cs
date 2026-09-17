namespace SoccerManager.Application.Commands.Manager.CreateManager;

/// <summary>
/// Maps <see cref="CreateManagerRequest"/> instances to domain entities.
/// </summary>
public static class CreateManagerMapper
{
    /// <summary>
    /// Creates a new manager entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the manager values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the caller, who becomes the manager and is also stamped as its creator.</param>
    /// <returns>A new, unsaved manager entity.</returns>
    public static SoccerManager.Domain.Entities.Manager ToDomainEntity(CreateManagerRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Manager
        {
            UserId = userId,
            ImageUrl = request.ImageUrl,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
