using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Player.GetPlayers;

/// <summary>
/// Represents the result of a <see cref="GetPlayersRequest"/> query.
/// </summary>
public sealed record GetPlayersResponse
{
    /// <summary>
    /// The list of all players.
    /// </summary>
    public required List<PlayerDto> Data { get; init; }
}
