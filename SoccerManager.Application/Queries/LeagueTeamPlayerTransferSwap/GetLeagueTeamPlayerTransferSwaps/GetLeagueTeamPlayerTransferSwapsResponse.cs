using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwaps;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayerTransferSwapsRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayerTransferSwapsResponse
{
    /// <summary>
    /// The list of swaps of the requested transfer.
    /// </summary>
    public required List<LeagueTeamPlayerTransferSwapDto> Data { get; init; }
}
