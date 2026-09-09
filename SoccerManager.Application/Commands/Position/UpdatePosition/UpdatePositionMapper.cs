namespace SoccerManager.Application.Commands.Position.UpdatePosition;

/// <summary>
/// Applies <see cref="UpdatePositionRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdatePositionMapper
{
    /// <summary>
    /// Applies the request values to the given position, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new position values.</param>
    /// <param name="position">The loaded position entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdatePositionRequest request, SoccerManager.Domain.Entities.Position position, DateTime now, Guid userId)
    {
        position.Name = request.Name;
        position.Area = request.Area;
        position.Side = request.Side;
        position.ModifiedAt = now;
        position.ModifiedBy = userId;
    }
}
