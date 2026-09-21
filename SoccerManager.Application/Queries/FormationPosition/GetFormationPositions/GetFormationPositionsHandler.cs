using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.FormationPosition.GetFormationPositions;

/// <summary>
/// Handles <see cref="GetFormationPositionsRequest"/> queries.
/// </summary>
public class GetFormationPositionsHandler : IRequestHandler<GetFormationPositionsRequest, GetFormationPositionsResponse>
{
    private readonly IFormationPositionRepository _formationPositionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFormationPositionsHandler"/> class.
    /// </summary>
    /// <param name="formationPositionRepository">The repository used to load formation positions.</param>
    public GetFormationPositionsHandler(IFormationPositionRepository formationPositionRepository)
    {
        _formationPositionRepository = formationPositionRepository;
    }

    /// <summary>
    /// Retrieves every position slot belonging to the requested formation.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching formation position slots.</returns>
    public async Task<GetFormationPositionsResponse> Handle(GetFormationPositionsRequest request, CancellationToken cancellationToken)
    {
        // No formation-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IFormationRepository out of a
        // handler that only needs IFormationPositionRepository.
        var formationPositions = await _formationPositionRepository.GetByFormationIdAsync(request.FormationId);

        return new GetFormationPositionsResponse { Data = formationPositions.Select(GetFormationPositionsMapper.ToDto).ToList() };
    }
}
