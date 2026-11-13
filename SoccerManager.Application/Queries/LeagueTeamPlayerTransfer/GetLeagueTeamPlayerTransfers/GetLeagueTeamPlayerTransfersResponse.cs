using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransfers;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayerTransfersRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayerTransfersResponse
{
    /// <summary>
    /// The list of transfers of the requested squad row, the latest first.
    /// </summary>
    public required List<LeagueTeamPlayerTransferDto> Data { get; init; }
}
