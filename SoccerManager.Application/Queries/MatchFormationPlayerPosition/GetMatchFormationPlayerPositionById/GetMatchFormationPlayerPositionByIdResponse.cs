using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositionById;

/// <summary>
/// Represents the result of a <see cref="GetMatchFormationPlayerPositionByIdRequest"/> query.
/// </summary>
public sealed record GetMatchFormationPlayerPositionByIdResponse
{
    /// <summary>
    /// The requested match formation player position.
    /// </summary>
    public required MatchFormationPlayerPositionDto Data { get; init; }
}
