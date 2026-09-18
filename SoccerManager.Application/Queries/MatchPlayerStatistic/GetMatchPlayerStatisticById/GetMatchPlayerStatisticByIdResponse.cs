using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatisticById;

/// <summary>
/// Represents the result of a <see cref="GetMatchPlayerStatisticByIdRequest"/> query.
/// </summary>
public sealed record GetMatchPlayerStatisticByIdResponse
{
    /// <summary>
    /// The requested match player statistic.
    /// </summary>
    public required MatchPlayerStatisticDto Data { get; init; }
}
