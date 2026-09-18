using MediatR;

namespace SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatistics;

/// <summary>
/// Represents a request to retrieve every team statistic recorded for a match.
/// </summary>
public sealed record GetMatchTeamStatisticsRequest : IRequest<GetMatchTeamStatisticsResponse>
{
    /// <summary>
    /// The identifier of the match whose team statistics are being requested.
    /// </summary>
    public int MatchId { get; init; }
}
