using MediatR;

namespace SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVoteById;

/// <summary>
/// Represents a request to retrieve a single vote by identifier.
/// </summary>
public sealed record GetManagerPlayerRatingVoteByIdRequest : IRequest<GetManagerPlayerRatingVoteByIdResponse>
{
    /// <summary>
    /// The identifier of the vote to retrieve.
    /// </summary>
    public int Id { get; init; }
}
