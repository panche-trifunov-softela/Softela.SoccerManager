using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltyAfterExtraTimeById;

/// <summary>
/// Represents the result of a <see cref="GetPenaltyAfterExtraTimeByIdRequest"/> query.
/// </summary>
public sealed record GetPenaltyAfterExtraTimeByIdResponse
{
    /// <summary>
    /// The requested penalty after extra time.
    /// </summary>
    public required PenaltyAfterExtraTimeDto Data { get; init; }
}
