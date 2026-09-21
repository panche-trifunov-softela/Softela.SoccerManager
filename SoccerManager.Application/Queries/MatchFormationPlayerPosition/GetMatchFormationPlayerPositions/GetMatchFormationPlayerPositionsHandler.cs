using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositions;

/// <summary>
/// Handles <see cref="GetMatchFormationPlayerPositionsRequest"/> queries.
/// </summary>
public class GetMatchFormationPlayerPositionsHandler : IRequestHandler<GetMatchFormationPlayerPositionsRequest, GetMatchFormationPlayerPositionsResponse>
{
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchFormationPlayerPositionsHandler"/> class.
    /// </summary>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to load match formation player positions.</param>
    public GetMatchFormationPlayerPositionsHandler(IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository)
    {
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
    }

    /// <summary>
    /// Retrieves every lineup slot recorded for the requested match.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching lineup slots.</returns>
    public async Task<GetMatchFormationPlayerPositionsResponse> Handle(GetMatchFormationPlayerPositionsRequest request, CancellationToken cancellationToken)
    {
        // No match-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IMatchRepository out of a
        // handler that only needs IMatchFormationPlayerPositionRepository.
        var matchFormationPlayerPositions = await _matchFormationPlayerPositionRepository.GetByMatchIdAsync(request.MatchId);

        return new GetMatchFormationPlayerPositionsResponse { Data = matchFormationPlayerPositions.Select(GetMatchFormationPlayerPositionsMapper.ToDto).ToList() };
    }
}
