using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwapById;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayerTransferSwapByIdRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayerTransferSwapByIdResponse
{
    /// <summary>
    /// The requested swap.
    /// </summary>
    public required LeagueTeamPlayerTransferSwapDto Data { get; init; }
}
