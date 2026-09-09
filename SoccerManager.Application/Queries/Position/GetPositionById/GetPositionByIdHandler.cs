using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Position.GetPositionById;

/// <summary>
/// Handles <see cref="GetPositionByIdRequest"/> queries.
/// </summary>
public class GetPositionByIdHandler : IRequestHandler<GetPositionByIdRequest, GetPositionByIdResponse>
{
    private readonly IPositionRepository _positionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPositionByIdHandler"/> class.
    /// </summary>
    /// <param name="positionRepository">The repository used to load positions.</param>
    public GetPositionByIdHandler(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    /// <summary>
    /// Retrieves the position identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested position.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no position with the given identifier exists.</exception>
    public async Task<GetPositionByIdResponse> Handle(GetPositionByIdRequest request, CancellationToken cancellationToken)
    {
        var position = await _positionRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Position {request.Id} not found.");

        return new GetPositionByIdResponse { Data = GetPositionByIdMapper.ToDto(position) };
    }
}
