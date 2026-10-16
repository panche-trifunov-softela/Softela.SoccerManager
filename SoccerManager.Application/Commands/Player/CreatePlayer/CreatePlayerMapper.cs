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
            NationalTeamId = request.NationalTeamId,
            TeamId = request.TeamId,
            TransfermarktId = request.TransfermarktId,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }

    /// <summary>
    /// Creates the new player's first rating, which is its current one.
    /// </summary>
    /// <param name="playerId">The identifier of the player the rating belongs to.</param>
    /// <param name="request">The request carrying the rating value.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the player.</param>
    /// <returns>A new, unsaved current rating entity.</returns>
    public static SoccerManager.Domain.Entities.PlayerRating ToPlayerRating(int playerId, CreatePlayerRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.PlayerRating
        {
            PlayerId = playerId,
            Rating = request.Rating,
            IsCurrent = true,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
