using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTactics;

/// <summary>
/// Represents the result of a <see cref="GetMatchTeamTacticsRequest"/> query.
/// </summary>
public sealed record GetMatchTeamTacticsResponse
{
    /// <summary>
    /// The list of team tactics recorded for the requested match.
    /// </summary>
    public required List<MatchTeamTacticDto> Data { get; init; }
}
