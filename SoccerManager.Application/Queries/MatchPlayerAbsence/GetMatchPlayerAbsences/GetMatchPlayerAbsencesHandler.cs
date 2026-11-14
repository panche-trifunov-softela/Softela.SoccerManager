using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsences;

/// <summary>
/// Handles <see cref="GetMatchPlayerAbsencesRequest"/> queries.
/// </summary>
public class GetMatchPlayerAbsencesHandler : IRequestHandler<GetMatchPlayerAbsencesRequest, GetMatchPlayerAbsencesResponse>
{
    private readonly IMatchPlayerAbsenceRepository _matchPlayerAbsenceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchPlayerAbsencesHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerAbsenceRepository">The repository used to load match player absences.</param>
    public GetMatchPlayerAbsencesHandler(IMatchPlayerAbsenceRepository matchPlayerAbsenceRepository)
    {
        _matchPlayerAbsenceRepository = matchPlayerAbsenceRepository;
    }

    /// <summary>
    /// Retrieves every player absence recorded for the requested match.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching player absences.</returns>
    public async Task<GetMatchPlayerAbsencesResponse> Handle(GetMatchPlayerAbsencesRequest request, CancellationToken cancellationToken)
    {
        // No match-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IMatchRepository out of a
        // handler that only needs IMatchPlayerAbsenceRepository.
        var matchPlayerAbsences = await _matchPlayerAbsenceRepository.GetByMatchIdAsync(request.MatchId);

        return new GetMatchPlayerAbsencesResponse { Data = matchPlayerAbsences.Select(GetMatchPlayerAbsencesMapper.ToDto).ToList() };
    }
}
