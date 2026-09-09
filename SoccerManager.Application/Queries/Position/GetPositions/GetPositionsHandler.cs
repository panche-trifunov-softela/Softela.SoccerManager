using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Position.GetPositions;

/// <summary>
/// Handles <see cref="GetPositionsRequest"/> queries.
/// </summary>
public class GetPositionsHandler : IRequestHandler<GetPositionsRequest, GetPositionsResponse>
{
    private readonly IPositionRepository _positionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPositionsHandler"/> class.
    /// </summary>
    /// <param name="positionRepository">The repository used to load positions.</param>
    public GetPositionsHandler(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    /// <summary>
    /// Retrieves all positions.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every position.</returns>
    public async Task<GetPositionsResponse> Handle(GetPositionsRequest request, CancellationToken cancellationToken)
    {
        var positions = await _positionRepository.GetAllAsync();

        return new GetPositionsResponse { Data = positions.Select(GetPositionsMapper.ToDto).ToList() };
    }
}
