using MediatR;

namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.UpdateManagerPlayerRatingVote;

/// <summary>
/// Represents a request to change the direction of one of the calling manager's votes.
/// </summary>
public sealed record UpdateManagerPlayerRatingVoteRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the vote to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Whether the vote is up; false means down.
    /// </summary>
    public bool UpOrDown { get; init; }
}
