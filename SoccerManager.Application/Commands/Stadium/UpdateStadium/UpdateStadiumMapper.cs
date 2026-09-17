namespace SoccerManager.Application.Commands.Stadium.UpdateStadium;

/// <summary>
/// Applies <see cref="UpdateStadiumRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateStadiumMapper
{
    /// <summary>
    /// Applies the request values to the given stadium, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new stadium values.</param>
    /// <param name="stadium">The loaded stadium entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateStadiumRequest request, SoccerManager.Domain.Entities.Stadium stadium, DateTime now, Guid userId)
    {
        stadium.Name = request.Name;
        stadium.ImageUrl = request.ImageUrl;
        stadium.Size = request.Size;
        stadium.ModifiedAt = now;
        stadium.ModifiedBy = userId;
    }
}
