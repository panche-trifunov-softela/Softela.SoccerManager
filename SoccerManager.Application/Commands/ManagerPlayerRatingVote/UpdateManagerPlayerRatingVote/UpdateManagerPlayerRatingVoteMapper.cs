namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.UpdateManagerPlayerRatingVote;

/// <summary>
/// Applies <see cref="UpdateManagerPlayerRatingVoteRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateManagerPlayerRatingVoteMapper
{
    /// <summary>
    /// Applies the new direction to the given vote, preserving its creation metadata and the rating it was cast against.
    /// </summary>
    /// <param name="request">The request carrying the new direction.</param>
    /// <param name="managerPlayerRatingVote">The loaded vote entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateManagerPlayerRatingVoteRequest request, SoccerManager.Domain.Entities.ManagerPlayerRatingVote managerPlayerRatingVote, DateTime now, Guid userId)
    {
        managerPlayerRatingVote.UpOrDown = request.UpOrDown;
        managerPlayerRatingVote.ModifiedAt = now;
        managerPlayerRatingVote.ModifiedBy = userId;
    }
}
