using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Player.GetPlayerById;

/// <summary>
/// Represents the result of a <see cref="GetPlayerByIdRequest"/> query.
/// </summary>
public sealed record GetPlayerByIdResponse
{
    /// <summary>
    /// The requested player.
    /// </summary>
    public required PlayerDto Data { get; init; }
}
