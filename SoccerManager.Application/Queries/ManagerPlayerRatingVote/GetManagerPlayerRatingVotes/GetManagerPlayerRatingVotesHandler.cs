using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVotes;

/// <summary>
/// Handles <see cref="GetManagerPlayerRatingVotesRequest"/> queries.
/// </summary>
public class GetManagerPlayerRatingVotesHandler : IRequestHandler<GetManagerPlayerRatingVotesRequest, GetManagerPlayerRatingVotesResponse>
{
    private readonly IManagerPlayerRatingVoteRepository _managerPlayerRatingVoteRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetManagerPlayerRatingVotesHandler"/> class.
    /// </summary>
    /// <param name="managerPlayerRatingVoteRepository">The repository used to load votes.</param>
    public GetManagerPlayerRatingVotesHandler(IManagerPlayerRatingVoteRepository managerPlayerRatingVoteRepository)
    {
        _managerPlayerRatingVoteRepository = managerPlayerRatingVoteRepository;
    }

    /// <summary>
    /// Retrieves every vote on the requested player, newest first.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching votes.</returns>
    public async Task<GetManagerPlayerRatingVotesResponse> Handle(GetManagerPlayerRatingVotesRequest request, CancellationToken cancellationToken)
    {
        // No player-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IPlayerRepository out of a
        // handler that only needs IManagerPlayerRatingVoteRepository.
        var managerPlayerRatingVotes = await _managerPlayerRatingVoteRepository.GetByPlayerIdAsync(request.PlayerId);

        return new GetManagerPlayerRatingVotesResponse { Data = managerPlayerRatingVotes.Select(GetManagerPlayerRatingVotesMapper.ToDto).ToList() };
    }
}
