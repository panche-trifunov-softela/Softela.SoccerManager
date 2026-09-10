using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositionById;

/// <summary>
/// Represents the result of a <see cref="GetPlayerPositionByIdRequest"/> query.
/// </summary>
public sealed record GetPlayerPositionByIdResponse
{
    /// <summary>
    /// The requested player position rating.
    /// </summary>
    public required PlayerPositionDto Data { get; init; }
}
