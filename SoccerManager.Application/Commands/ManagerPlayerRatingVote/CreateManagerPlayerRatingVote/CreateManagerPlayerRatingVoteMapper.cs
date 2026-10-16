namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.CreateManagerPlayerRatingVote;

/// <summary>
/// Maps <see cref="CreateManagerPlayerRatingVoteRequest"/> instances to domain entities.
/// </summary>
public static class CreateManagerPlayerRatingVoteMapper
{
    /// <summary>
    /// Creates a new vote entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the player and the direction of the vote.</param>
    /// <param name="managerId">The identifier of the voting manager, taken from the caller's manager profile.</param>
    /// <param name="playerRatingId">The identifier of the player's current rating, which the vote is cast on.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user casting the vote.</param>
    /// <returns>A new, unsaved vote entity.</returns>
    public static SoccerManager.Domain.Entities.ManagerPlayerRatingVote ToDomainEntity(CreateManagerPlayerRatingVoteRequest request, int managerId, int playerRatingId, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.ManagerPlayerRatingVote
        {
            ManagerId = managerId,
            PlayerRatingId = playerRatingId,
            UpOrDown = request.UpOrDown,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
