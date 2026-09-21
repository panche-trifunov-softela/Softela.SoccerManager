using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.FormationPosition.GetFormationPositions;

/// <summary>
/// Represents the result of a <see cref="GetFormationPositionsRequest"/> query.
/// </summary>
public sealed record GetFormationPositionsResponse
{
    /// <summary>
    /// The list of position slots belonging to the requested formation.
    /// </summary>
    public required List<FormationPositionDto> Data { get; init; }
}
