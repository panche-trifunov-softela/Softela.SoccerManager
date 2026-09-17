using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Match.GetMatchById;

/// <summary>
/// Handles <see cref="GetMatchByIdRequest"/> queries.
/// </summary>
public class GetMatchByIdHandler : IRequestHandler<GetMatchByIdRequest, GetMatchByIdResponse>
{
    private readonly IMatchRepository _matchRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchByIdHandler"/> class.
    /// </summary>
    /// <param name="matchRepository">The repository used to load matches.</param>
    public GetMatchByIdHandler(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    /// <summary>
    /// Retrieves the match identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested match.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match with the given identifier exists.</exception>
    public async Task<GetMatchByIdResponse> Handle(GetMatchByIdRequest request, CancellationToken cancellationToken)
    {
        var match = await _matchRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Match {request.Id} not found.");

        return new GetMatchByIdResponse { Data = GetMatchByIdMapper.ToDto(match) };
    }
}
