namespace SoccerManager.Application.Commands.Manager.UpdateManager;

/// <summary>
/// Applies <see cref="UpdateManagerRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateManagerMapper
{
    /// <summary>
    /// Applies the request values to the given manager, preserving its creation metadata and its immutable UserId.
    /// </summary>
    /// <param name="request">The request carrying the new manager values.</param>
    /// <param name="manager">The loaded manager entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateManagerRequest request, SoccerManager.Domain.Entities.Manager manager, DateTime now, Guid userId)
    {
        manager.ImageUrl = request.ImageUrl;
        manager.ModifiedAt = now;
        manager.ModifiedBy = userId;
    }
}
