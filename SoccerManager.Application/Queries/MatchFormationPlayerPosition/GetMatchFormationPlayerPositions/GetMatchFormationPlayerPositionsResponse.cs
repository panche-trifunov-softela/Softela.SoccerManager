using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositions;

/// <summary>
/// Represents the result of a <see cref="GetMatchFormationPlayerPositionsRequest"/> query.
/// </summary>
public sealed record GetMatchFormationPlayerPositionsResponse
{
    /// <summary>
    /// The lineup slots recorded for the requested match.
    /// </summary>
    public required List<MatchFormationPlayerPositionDto> Data { get; init; }
}
