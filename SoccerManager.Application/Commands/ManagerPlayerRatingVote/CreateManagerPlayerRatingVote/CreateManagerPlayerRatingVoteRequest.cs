using MediatR;

namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.CreateManagerPlayerRatingVote;

/// <summary>
/// Command to cast a vote, as the calling manager, on a player's current rating.
/// </summary>
public sealed record CreateManagerPlayerRatingVoteRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the player to vote on.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// Whether the vote is up; false means down.
    /// </summary>
    public bool UpOrDown { get; init; }
}
