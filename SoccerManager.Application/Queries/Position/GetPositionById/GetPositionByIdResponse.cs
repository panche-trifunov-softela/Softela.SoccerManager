using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Position.GetPositionById;

/// <summary>
/// Represents the result of a <see cref="GetPositionByIdRequest"/> query.
/// </summary>
public sealed record GetPositionByIdResponse
{
    /// <summary>
    /// The requested position.
    /// </summary>
    public required PositionDto Data { get; init; }
}
