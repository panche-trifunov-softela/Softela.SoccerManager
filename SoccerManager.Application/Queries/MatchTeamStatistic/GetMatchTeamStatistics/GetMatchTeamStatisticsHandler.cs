using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatistics;

/// <summary>
/// Handles <see cref="GetMatchTeamStatisticsRequest"/> queries.
/// </summary>
public class GetMatchTeamStatisticsHandler : IRequestHandler<GetMatchTeamStatisticsRequest, GetMatchTeamStatisticsResponse>
{
    private readonly IMatchTeamStatisticRepository _matchTeamStatisticRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchTeamStatisticsHandler"/> class.
    /// </summary>
    /// <param name="matchTeamStatisticRepository">The repository used to load match team statistics.</param>
    public GetMatchTeamStatisticsHandler(IMatchTeamStatisticRepository matchTeamStatisticRepository)
    {
        _matchTeamStatisticRepository = matchTeamStatisticRepository;
    }

    /// <summary>
    /// Retrieves every team statistic recorded for the requested match.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching team statistics.</returns>
    public async Task<GetMatchTeamStatisticsResponse> Handle(GetMatchTeamStatisticsRequest request, CancellationToken cancellationToken)
    {
        // No match-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IMatchRepository out of a
        // handler that only needs IMatchTeamStatisticRepository.
        var matchTeamStatistics = await _matchTeamStatisticRepository.GetByMatchIdAsync(request.MatchId);

        return new GetMatchTeamStatisticsResponse { Data = matchTeamStatistics.Select(GetMatchTeamStatisticsMapper.ToDto).ToList() };
    }
}
