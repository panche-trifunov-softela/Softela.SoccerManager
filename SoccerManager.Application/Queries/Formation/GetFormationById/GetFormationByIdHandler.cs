using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Formation.GetFormationById;

/// <summary>
/// Handles <see cref="GetFormationByIdRequest"/> queries.
/// </summary>
public class GetFormationByIdHandler : IRequestHandler<GetFormationByIdRequest, GetFormationByIdResponse>
{
    private readonly IFormationRepository _formationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFormationByIdHandler"/> class.
    /// </summary>
    /// <param name="formationRepository">The repository used to load formations.</param>
    public GetFormationByIdHandler(IFormationRepository formationRepository)
    {
        _formationRepository = formationRepository;
    }

    /// <summary>
    /// Retrieves the formation identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested formation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no formation with the given identifier exists.</exception>
    public async Task<GetFormationByIdResponse> Handle(GetFormationByIdRequest request, CancellationToken cancellationToken)
    {
        var formation = await _formationRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Formation {request.Id} not found.");

        return new GetFormationByIdResponse { Data = GetFormationByIdMapper.ToDto(formation) };
    }
}
