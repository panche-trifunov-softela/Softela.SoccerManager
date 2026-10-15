using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVoteById;

/// <summary>
/// Represents the result of a <see cref="GetManagerPlayerRatingVoteByIdRequest"/> query.
/// </summary>
public sealed record GetManagerPlayerRatingVoteByIdResponse
{
    /// <summary>
    /// The requested vote.
    /// </summary>
    public required ManagerPlayerRatingVoteDto Data { get; init; }
}
