namespace SoccerManager.Application.Commands.Manager.EnsureMyManager;

/// <summary>
/// Maps an <see cref="EnsureMyManagerRequest"/> to the manager profile it creates when the caller has none.
/// </summary>
public static class EnsureMyManagerMapper
{
    /// <summary>
    /// Creates the caller's manager profile with no image, as it is stored on a first login.
    /// </summary>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the authenticated caller, who owns the profile.</param>
    /// <returns>A new, unsaved manager entity.</returns>
    public static SoccerManager.Domain.Entities.Manager ToDomainEntity(DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Manager
        {
            UserId = userId,
            ImageUrl = null,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
