namespace SoccerManager.Application.Commands.Player.CreatePlayer;

/// <summary>
/// Maps <see cref="CreatePlayerRequest"/> instances to domain entities.
/// </summary>
public static class CreatePlayerMapper
{
    /// <summary>
    /// Creates a new player entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the player values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the player.</param>
    /// <returns>A new, unsaved player entity.</returns>
    public static SoccerManager.Domain.Entities.Player ToDomainEntity(CreatePlayerRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Player
        {
            Name = request.Name,
            DateOfBirth = request.DateOfBirth,
            Rating = request.Rating,
            Value = request.Value,
            Wage = request.Wage,
            ImageUrl = request.ImageUrl,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
