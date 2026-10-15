using MediatR;

namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.DeleteManagerPlayerRatingVote;

/// <summary>
/// Represents a request to withdraw one of the calling manager's votes.
/// </summary>
public sealed record DeleteManagerPlayerRatingVoteRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the vote to delete.
    /// </summary>
    public int Id { get; init; }
}
