using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTacticById;

/// <summary>
/// Represents the result of a <see cref="GetMatchTeamTacticByIdRequest"/> query.
/// </summary>
public sealed record GetMatchTeamTacticByIdResponse
{
    /// <summary>
    /// The requested match team tactic.
    /// </summary>
    public required MatchTeamTacticDto Data { get; init; }
}
