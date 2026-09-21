using MediatR;

namespace SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositions;

/// <summary>
/// Represents a request to retrieve every lineup slot recorded for a match, for both teams.
/// </summary>
public sealed record GetMatchFormationPlayerPositionsRequest : IRequest<GetMatchFormationPlayerPositionsResponse>
{
    /// <summary>
    /// The identifier of the match whose lineup is being requested.
    /// </summary>
    public int MatchId { get; init; }
}
