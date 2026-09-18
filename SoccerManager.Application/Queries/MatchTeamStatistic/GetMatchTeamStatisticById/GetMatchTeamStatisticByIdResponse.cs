using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatisticById;

/// <summary>
/// Represents the result of a <see cref="GetMatchTeamStatisticByIdRequest"/> query.
/// </summary>
public sealed record GetMatchTeamStatisticByIdResponse
{
    /// <summary>
    /// The requested match team statistic.
    /// </summary>
    public required MatchTeamStatisticDto Data { get; init; }
}
