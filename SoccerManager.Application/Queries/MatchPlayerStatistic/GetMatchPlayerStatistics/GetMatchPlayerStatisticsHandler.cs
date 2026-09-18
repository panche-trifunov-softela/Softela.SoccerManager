using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatistics;

/// <summary>
/// Handles <see cref="GetMatchPlayerStatisticsRequest"/> queries.
/// </summary>
public class GetMatchPlayerStatisticsHandler : IRequestHandler<GetMatchPlayerStatisticsRequest, GetMatchPlayerStatisticsResponse>
{
    private readonly IMatchPlayerStatisticRepository _matchPlayerStatisticRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchPlayerStatisticsHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerStatisticRepository">The repository used to load match player statistics.</param>
    public GetMatchPlayerStatisticsHandler(IMatchPlayerStatisticRepository matchPlayerStatisticRepository)
    {
        _matchPlayerStatisticRepository = matchPlayerStatisticRepository;
    }

    /// <summary>
    /// Retrieves every player statistic recorded for the requested match.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching player statistics.</returns>
    public async Task<GetMatchPlayerStatisticsResponse> Handle(GetMatchPlayerStatisticsRequest request, CancellationToken cancellationToken)
    {
        // No match-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IMatchRepository out of a
        // handler that only needs IMatchPlayerStatisticRepository.
        var matchPlayerStatistics = await _matchPlayerStatisticRepository.GetByMatchIdAsync(request.MatchId);

        return new GetMatchPlayerStatisticsResponse { Data = matchPlayerStatistics.Select(GetMatchPlayerStatisticsMapper.ToDto).ToList() };
    }
}
