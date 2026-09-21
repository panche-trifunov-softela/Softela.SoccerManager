using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.FormationPosition.GetFormationPositionById;

/// <summary>
/// Handles <see cref="GetFormationPositionByIdRequest"/> queries.
/// </summary>
public class GetFormationPositionByIdHandler : IRequestHandler<GetFormationPositionByIdRequest, GetFormationPositionByIdResponse>
{
    private readonly IFormationPositionRepository _formationPositionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFormationPositionByIdHandler"/> class.
    /// </summary>
    /// <param name="formationPositionRepository">The repository used to load formation positions.</param>
    public GetFormationPositionByIdHandler(IFormationPositionRepository formationPositionRepository)
    {
        _formationPositionRepository = formationPositionRepository;
    }

    /// <summary>
    /// Retrieves the formation position slot identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested formation position slot.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no formation position with the given identifier exists.</exception>
    public async Task<GetFormationPositionByIdResponse> Handle(GetFormationPositionByIdRequest request, CancellationToken cancellationToken)
    {
        var formationPosition = await _formationPositionRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Formation position {request.Id} not found.");

        return new GetFormationPositionByIdResponse { Data = GetFormationPositionByIdMapper.ToDto(formationPosition) };
    }
}
