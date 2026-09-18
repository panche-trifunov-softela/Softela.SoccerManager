using MediatR;

namespace SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatistics;

/// <summary>
/// Represents a request to retrieve every player statistic recorded for a match.
/// </summary>
public sealed record GetMatchPlayerStatisticsRequest : IRequest<GetMatchPlayerStatisticsResponse>
{
    /// <summary>
    /// The identifier of the match whose player statistics are being requested.
    /// </summary>
    public int MatchId { get; init; }
}
