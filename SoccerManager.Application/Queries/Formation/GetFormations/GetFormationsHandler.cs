using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Formation.GetFormations;

/// <summary>
/// Handles <see cref="GetFormationsRequest"/> queries.
/// </summary>
public class GetFormationsHandler : IRequestHandler<GetFormationsRequest, GetFormationsResponse>
{
    private readonly IFormationRepository _formationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFormationsHandler"/> class.
    /// </summary>
    /// <param name="formationRepository">The repository used to load formations.</param>
    public GetFormationsHandler(IFormationRepository formationRepository)
    {
        _formationRepository = formationRepository;
    }

    /// <summary>
    /// Retrieves all formations.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every formation.</returns>
    public async Task<GetFormationsResponse> Handle(GetFormationsRequest request, CancellationToken cancellationToken)
    {
        var formations = await _formationRepository.GetAllAsync();

        return new GetFormationsResponse { Data = formations.Select(GetFormationsMapper.ToDto).ToList() };
    }
}
