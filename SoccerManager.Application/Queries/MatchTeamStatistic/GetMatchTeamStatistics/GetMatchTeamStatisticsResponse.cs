using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatistics;

/// <summary>
/// Represents the result of a <see cref="GetMatchTeamStatisticsRequest"/> query.
/// </summary>
public sealed record GetMatchTeamStatisticsResponse
{
    /// <summary>
    /// The list of team statistics recorded for the requested match.
    /// </summary>
    public required List<MatchTeamStatisticDto> Data { get; init; }
}
