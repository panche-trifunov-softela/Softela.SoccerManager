using MediatR;

namespace SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltiesAfterExtraTime;

/// <summary>
/// Represents a request to retrieve every shootout kick recorded for a match.
/// </summary>
public sealed record GetPenaltiesAfterExtraTimeRequest : IRequest<GetPenaltiesAfterExtraTimeResponse>
{
    /// <summary>
    /// The identifier of the match whose shootout kicks are being requested.
    /// </summary>
    public int MatchId { get; init; }
}
