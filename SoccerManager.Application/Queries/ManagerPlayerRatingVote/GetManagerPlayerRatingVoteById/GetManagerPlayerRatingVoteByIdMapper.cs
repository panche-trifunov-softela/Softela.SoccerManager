using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVoteById;

/// <summary>
/// Maps vote domain entities to <see cref="ManagerPlayerRatingVoteDto"/> instances.
/// </summary>
public static class GetManagerPlayerRatingVoteByIdMapper
{
    /// <summary>
    /// Converts a vote entity into its DTO representation.
    /// </summary>
    /// <param name="managerPlayerRatingVote">The vote entity to convert.</param>
    /// <returns>The corresponding <see cref="ManagerPlayerRatingVoteDto"/>.</returns>
    public static ManagerPlayerRatingVoteDto ToDto(SoccerManager.Domain.Entities.ManagerPlayerRatingVote managerPlayerRatingVote)
    {
        return new ManagerPlayerRatingVoteDto
        {
            Id = managerPlayerRatingVote.Id,
            ManagerId = managerPlayerRatingVote.ManagerId,
            PlayerRatingId = managerPlayerRatingVote.PlayerRatingId,
            UpOrDown = managerPlayerRatingVote.UpOrDown,
            CreatedAt = managerPlayerRatingVote.CreatedAt,
            ModifiedAt = managerPlayerRatingVote.ModifiedAt,
        };
    }
}
