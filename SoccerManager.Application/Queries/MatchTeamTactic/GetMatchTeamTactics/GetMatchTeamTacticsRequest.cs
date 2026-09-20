using MediatR;

namespace SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTactics;

/// <summary>
/// Represents a request to retrieve every team tactic recorded for a match.
/// </summary>
public sealed record GetMatchTeamTacticsRequest : IRequest<GetMatchTeamTacticsResponse>
{
    /// <summary>
    /// The identifier of the match whose team tactics are being requested.
    /// </summary>
    public int MatchId { get; init; }
}
