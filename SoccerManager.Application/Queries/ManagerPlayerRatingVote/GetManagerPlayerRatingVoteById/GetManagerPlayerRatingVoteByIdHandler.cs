using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVoteById;

/// <summary>
/// Handles <see cref="GetManagerPlayerRatingVoteByIdRequest"/> queries.
/// </summary>
public class GetManagerPlayerRatingVoteByIdHandler : IRequestHandler<GetManagerPlayerRatingVoteByIdRequest, GetManagerPlayerRatingVoteByIdResponse>
{
    private readonly IManagerPlayerRatingVoteRepository _managerPlayerRatingVoteRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetManagerPlayerRatingVoteByIdHandler"/> class.
    /// </summary>
    /// <param name="managerPlayerRatingVoteRepository">The repository used to load votes.</param>
    public GetManagerPlayerRatingVoteByIdHandler(IManagerPlayerRatingVoteRepository managerPlayerRatingVoteRepository)
    {
        _managerPlayerRatingVoteRepository = managerPlayerRatingVoteRepository;
    }

    /// <summary>
    /// Retrieves the vote identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested vote.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no vote with the given identifier exists.</exception>
    public async Task<GetManagerPlayerRatingVoteByIdResponse> Handle(GetManagerPlayerRatingVoteByIdRequest request, CancellationToken cancellationToken)
    {
        var managerPlayerRatingVote = await _managerPlayerRatingVoteRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Manager player rating vote {request.Id} not found.");

        return new GetManagerPlayerRatingVoteByIdResponse { Data = GetManagerPlayerRatingVoteByIdMapper.ToDto(managerPlayerRatingVote) };
    }
}
