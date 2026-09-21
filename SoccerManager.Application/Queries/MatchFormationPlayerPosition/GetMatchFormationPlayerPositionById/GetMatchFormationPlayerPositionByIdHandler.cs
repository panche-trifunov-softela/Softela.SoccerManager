using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositionById;

/// <summary>
/// Handles <see cref="GetMatchFormationPlayerPositionByIdRequest"/> queries.
/// </summary>
public class GetMatchFormationPlayerPositionByIdHandler : IRequestHandler<GetMatchFormationPlayerPositionByIdRequest, GetMatchFormationPlayerPositionByIdResponse>
{
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchFormationPlayerPositionByIdHandler"/> class.
    /// </summary>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to load match formation player positions.</param>
    public GetMatchFormationPlayerPositionByIdHandler(IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository)
    {
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
    }

    /// <summary>
    /// Retrieves the match formation player position identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested match formation player position.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match formation player position with the given identifier exists.</exception>
    public async Task<GetMatchFormationPlayerPositionByIdResponse> Handle(GetMatchFormationPlayerPositionByIdRequest request, CancellationToken cancellationToken)
    {
        var matchFormationPlayerPosition = await _matchFormationPlayerPositionRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Match formation player position {request.Id} not found.");

        return new GetMatchFormationPlayerPositionByIdResponse { Data = GetMatchFormationPlayerPositionByIdMapper.ToDto(matchFormationPlayerPosition) };
    }
}
