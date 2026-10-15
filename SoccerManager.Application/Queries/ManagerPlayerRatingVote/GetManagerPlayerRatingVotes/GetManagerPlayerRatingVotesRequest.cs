using MediatR;

namespace SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVotes;

/// <summary>
/// Represents a request to retrieve every vote on a player.
/// </summary>
public sealed record GetManagerPlayerRatingVotesRequest : IRequest<GetManagerPlayerRatingVotesResponse>
{
    /// <summary>
    /// The identifier of the player whose votes are being requested.
    /// </summary>
    public int PlayerId { get; init; }
}
