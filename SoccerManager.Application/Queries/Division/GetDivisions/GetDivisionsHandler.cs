using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Division.GetDivisions;

/// <summary>
/// Handles <see cref="GetDivisionsRequest"/> queries.
/// </summary>
public class GetDivisionsHandler : IRequestHandler<GetDivisionsRequest, GetDivisionsResponse>
{
    private readonly IDivisionRepository _divisionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetDivisionsHandler"/> class.
    /// </summary>
    /// <param name="divisionRepository">The repository used to load divisions.</param>
    public GetDivisionsHandler(IDivisionRepository divisionRepository)
    {
        _divisionRepository = divisionRepository;
    }

    /// <summary>
    /// Retrieves every division belonging to the requested league.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching divisions.</returns>
    public async Task<GetDivisionsResponse> Handle(GetDivisionsRequest request, CancellationToken cancellationToken)
    {
        // No league-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueRepository out of a
        // handler that only needs IDivisionRepository.
        var divisions = await _divisionRepository.GetByLeagueIdAsync(request.LeagueId);

        return new GetDivisionsResponse { Data = divisions.Select(GetDivisionsMapper.ToDto).ToList() };
    }
}
