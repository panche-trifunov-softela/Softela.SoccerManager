namespace SoccerManager.Application.Commands.Formation.UpdateFormation;

/// <summary>
/// Applies <see cref="UpdateFormationRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateFormationMapper
{
    /// <summary>
    /// Applies the request values to the given formation, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new formation values.</param>
    /// <param name="formation">The loaded formation entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateFormationRequest request, SoccerManager.Domain.Entities.Formation formation, DateTime now, Guid userId)
    {
        formation.Name = request.Name;
        formation.ModifiedAt = now;
        formation.ModifiedBy = userId;
    }
}
