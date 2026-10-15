using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVotes;

/// <summary>
/// Represents the result of a <see cref="GetManagerPlayerRatingVotesRequest"/> query.
/// </summary>
public sealed record GetManagerPlayerRatingVotesResponse
{
    /// <summary>
    /// The list of votes on the requested player, newest first.
    /// </summary>
    public required List<ManagerPlayerRatingVoteDto> Data { get; init; }
}
