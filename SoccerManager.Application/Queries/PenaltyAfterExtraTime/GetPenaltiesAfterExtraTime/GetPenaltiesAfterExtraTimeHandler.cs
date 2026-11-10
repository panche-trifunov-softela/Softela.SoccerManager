using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltiesAfterExtraTime;

/// <summary>
/// Handles <see cref="GetPenaltiesAfterExtraTimeRequest"/> queries.
/// </summary>
public class GetPenaltiesAfterExtraTimeHandler : IRequestHandler<GetPenaltiesAfterExtraTimeRequest, GetPenaltiesAfterExtraTimeResponse>
{
    private readonly IPenaltyAfterExtraTimeRepository _penaltyAfterExtraTimeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPenaltiesAfterExtraTimeHandler"/> class.
    /// </summary>
    /// <param name="penaltyAfterExtraTimeRepository">The repository used to load penalties after extra time.</param>
    public GetPenaltiesAfterExtraTimeHandler(IPenaltyAfterExtraTimeRepository penaltyAfterExtraTimeRepository)
    {
        _penaltyAfterExtraTimeRepository = penaltyAfterExtraTimeRepository;
    }

    /// <summary>
    /// Retrieves every shootout kick recorded for the requested match, in kick order.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching shootout kicks.</returns>
    public async Task<GetPenaltiesAfterExtraTimeResponse> Handle(GetPenaltiesAfterExtraTimeRequest request, CancellationToken cancellationToken)
    {
        // No match-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IMatchRepository out of a
        // handler that only needs IPenaltyAfterExtraTimeRepository.
        var penaltiesAfterExtraTime = await _penaltyAfterExtraTimeRepository.GetByMatchIdAsync(request.MatchId);

        return new GetPenaltiesAfterExtraTimeResponse { Data = penaltiesAfterExtraTime.Select(GetPenaltiesAfterExtraTimeMapper.ToDto).ToList() };
    }
}
