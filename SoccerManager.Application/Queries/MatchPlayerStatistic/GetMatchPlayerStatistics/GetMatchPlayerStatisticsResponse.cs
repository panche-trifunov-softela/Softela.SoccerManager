using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatistics;

/// <summary>
/// Represents the result of a <see cref="GetMatchPlayerStatisticsRequest"/> query.
/// </summary>
public sealed record GetMatchPlayerStatisticsResponse
{
    /// <summary>
    /// The list of player statistics recorded for the requested match.
    /// </summary>
    public required List<MatchPlayerStatisticDto> Data { get; init; }
}
