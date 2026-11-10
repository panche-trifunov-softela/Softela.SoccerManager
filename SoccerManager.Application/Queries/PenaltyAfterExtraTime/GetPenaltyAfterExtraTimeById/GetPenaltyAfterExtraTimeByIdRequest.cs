using MediatR;

namespace SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltyAfterExtraTimeById;

/// <summary>
/// Represents a request to retrieve a single penalty after extra time by identifier.
/// </summary>
public sealed record GetPenaltyAfterExtraTimeByIdRequest : IRequest<GetPenaltyAfterExtraTimeByIdResponse>
{
    /// <summary>
    /// The identifier of the penalty after extra time to retrieve.
    /// </summary>
    public int Id { get; init; }
}
