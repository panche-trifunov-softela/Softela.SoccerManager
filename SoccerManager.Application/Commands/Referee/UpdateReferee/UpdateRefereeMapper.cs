namespace SoccerManager.Application.Commands.Referee.UpdateReferee;

/// <summary>
/// Applies <see cref="UpdateRefereeRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateRefereeMapper
{
    /// <summary>
    /// Applies the request values to the given referee, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new referee values.</param>
    /// <param name="referee">The loaded referee entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateRefereeRequest request, SoccerManager.Domain.Entities.Referee referee, DateTime now, Guid userId)
    {
        referee.Name = request.Name;
        referee.ImageUrl = request.ImageUrl;
        referee.Tolerance = request.Tolerance;
        referee.ModifiedAt = now;
        referee.ModifiedBy = userId;
    }
}
