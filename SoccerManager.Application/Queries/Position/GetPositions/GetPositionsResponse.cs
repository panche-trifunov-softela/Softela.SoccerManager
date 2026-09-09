using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Position.GetPositions;

/// <summary>
/// Represents the result of a <see cref="GetPositionsRequest"/> query.
/// </summary>
public sealed record GetPositionsResponse
{
    /// <summary>
    /// The list of all positions.
    /// </summary>
    public required List<PositionDto> Data { get; init; }
}
