using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltiesAfterExtraTime;

/// <summary>
/// Represents the result of a <see cref="GetPenaltiesAfterExtraTimeRequest"/> query.
/// </summary>
public sealed record GetPenaltiesAfterExtraTimeResponse
{
    /// <summary>
    /// The shootout kicks recorded for the requested match, in kick order.
    /// </summary>
    public required List<PenaltyAfterExtraTimeDto> Data { get; init; }
}
