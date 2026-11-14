using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsenceById;

/// <summary>
/// Handles <see cref="GetMatchPlayerAbsenceByIdRequest"/> queries.
/// </summary>
public class GetMatchPlayerAbsenceByIdHandler : IRequestHandler<GetMatchPlayerAbsenceByIdRequest, GetMatchPlayerAbsenceByIdResponse>
{
    private readonly IMatchPlayerAbsenceRepository _matchPlayerAbsenceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchPlayerAbsenceByIdHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerAbsenceRepository">The repository used to load match player absences.</param>
    public GetMatchPlayerAbsenceByIdHandler(IMatchPlayerAbsenceRepository matchPlayerAbsenceRepository)
    {
        _matchPlayerAbsenceRepository = matchPlayerAbsenceRepository;
    }

    /// <summary>
    /// Retrieves the match player absence identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested match player absence.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match player absence with the given identifier exists.</exception>
    public async Task<GetMatchPlayerAbsenceByIdResponse> Handle(GetMatchPlayerAbsenceByIdRequest request, CancellationToken cancellationToken)
    {
        var matchPlayerAbsence = await _matchPlayerAbsenceRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Match player absence {request.Id} not found.");

        return new GetMatchPlayerAbsenceByIdResponse { Data = GetMatchPlayerAbsenceByIdMapper.ToDto(matchPlayerAbsence) };
    }
}
