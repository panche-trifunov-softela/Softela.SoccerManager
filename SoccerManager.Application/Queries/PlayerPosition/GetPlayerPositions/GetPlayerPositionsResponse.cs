using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositions;

/// <summary>
/// Represents the result of a <see cref="GetPlayerPositionsRequest"/> query.
/// </summary>
public sealed record GetPlayerPositionsResponse
{
    /// <summary>
    /// The list of position ratings belonging to the requested player.
    /// </summary>
    public required List<PlayerPositionDto> Data { get; init; }
}
