namespace SoccerManager.Application.Commands.PlayerRating.CreatePlayerRating;

/// <summary>
/// Maps <see cref="CreatePlayerRatingRequest"/> instances to domain entities.
/// </summary>
public static class CreatePlayerRatingMapper
{
    /// <summary>
    /// Creates a new player rating entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the player rating values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the player rating.</param>
    /// <returns>A new, unsaved player rating entity, marked as the player's current rating.</returns>
    public static SoccerManager.Domain.Entities.PlayerRating ToDomainEntity(CreatePlayerRatingRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.PlayerRating
        {
            PlayerId = request.PlayerId,
            Rating = request.Rating,
            IsCurrent = true,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
