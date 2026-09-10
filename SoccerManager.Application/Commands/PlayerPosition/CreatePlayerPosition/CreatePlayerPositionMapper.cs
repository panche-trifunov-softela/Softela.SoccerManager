namespace SoccerManager.Application.Commands.PlayerPosition.CreatePlayerPosition;

/// <summary>
/// Maps <see cref="CreatePlayerPositionRequest"/> instances to domain entities.
/// </summary>
public static class CreatePlayerPositionMapper
{
    /// <summary>
    /// Creates a new player position entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the player position values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the player position.</param>
    /// <returns>A new, unsaved player position entity.</returns>
    public static SoccerManager.Domain.Entities.PlayerPosition ToDomainEntity(CreatePlayerPositionRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.PlayerPosition
        {
            PlayerId = request.PlayerId,
            PositionId = request.PositionId,
            Quality = request.Quality,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
