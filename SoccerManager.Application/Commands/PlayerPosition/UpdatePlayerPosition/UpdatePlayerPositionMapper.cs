namespace SoccerManager.Application.Commands.PlayerPosition.UpdatePlayerPosition;

/// <summary>
/// Applies <see cref="UpdatePlayerPositionRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdatePlayerPositionMapper
{
    /// <summary>
    /// Applies the request values to the given player position, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new player position values.</param>
    /// <param name="playerPosition">The loaded player position entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdatePlayerPositionRequest request, SoccerManager.Domain.Entities.PlayerPosition playerPosition, DateTime now, Guid userId)
    {
        playerPosition.PlayerId = request.PlayerId;
        playerPosition.PositionId = request.PositionId;
        playerPosition.Quality = request.Quality;
        playerPosition.ModifiedAt = now;
        playerPosition.ModifiedBy = userId;
    }
}
