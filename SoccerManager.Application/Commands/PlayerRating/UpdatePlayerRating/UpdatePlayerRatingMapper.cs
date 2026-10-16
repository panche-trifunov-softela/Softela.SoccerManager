namespace SoccerManager.Application.Commands.PlayerRating.UpdatePlayerRating;

/// <summary>
/// Applies <see cref="UpdatePlayerRatingRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdatePlayerRatingMapper
{
    /// <summary>
    /// Applies the corrected value to the given player rating, preserving its creation metadata and whether it is current.
    /// </summary>
    /// <param name="request">The request carrying the corrected rating value.</param>
    /// <param name="playerRating">The loaded player rating entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdatePlayerRatingRequest request, SoccerManager.Domain.Entities.PlayerRating playerRating, DateTime now, Guid userId)
    {
        playerRating.Rating = request.Rating;
        playerRating.ModifiedAt = now;
        playerRating.ModifiedBy = userId;
    }
}
