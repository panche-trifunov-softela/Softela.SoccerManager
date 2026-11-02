namespace SoccerManager.Application.Commands.FormationPosition.UpdateFormationPosition;

/// <summary>
/// Applies <see cref="UpdateFormationPositionRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateFormationPositionMapper
{
    /// <summary>
    /// Applies the request values to the given formation position, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new formation position values.</param>
    /// <param name="formationPosition">The loaded formation position entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateFormationPositionRequest request, SoccerManager.Domain.Entities.FormationPosition formationPosition, DateTime now, Guid userId)
    {
        formationPosition.FormationId = request.FormationId;
        formationPosition.PositionId = request.PositionId;
        formationPosition.SlotNumber = request.SlotNumber;

        // A reserve slot has no place on the pitch; a starter slot keeps its stored place unless the request sends a new one.
        if (request.SlotNumber > 11)
        {
            formationPosition.PitchX = null;
            formationPosition.PitchY = null;
        }
        else if (request.PitchX.HasValue)
        {
            formationPosition.PitchX = request.PitchX;
            formationPosition.PitchY = request.PitchY;
        }

        formationPosition.ModifiedAt = now;
        formationPosition.ModifiedBy = userId;
    }
}
