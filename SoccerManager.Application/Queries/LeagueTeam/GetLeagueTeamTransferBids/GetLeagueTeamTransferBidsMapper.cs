using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;
using SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBids;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamTransferBids;

/// <summary>
/// Maps a club's transfer bid rows to a <see cref="LeagueTeamTransferBidsDto"/>.
/// </summary>
public static class GetLeagueTeamTransferBidsMapper
{
    /// <summary>
    /// Converts the transfer bid rows and their swapped-player rows into the bids' DTO representation: the pending bids in
    /// one list and the accepted and completed ones in the other, each bid carrying its own offered players in row order.
    /// </summary>
    /// <param name="bids">The transfer bid rows returned by the stored procedure, in the order to keep.</param>
    /// <param name="swapPlayers">The offered-player rows returned by the stored procedure, in swap-row order.</param>
    /// <returns>The corresponding <see cref="LeagueTeamTransferBidsDto"/>, with both lists in the order of their rows.</returns>
    public static LeagueTeamTransferBidsDto ToDto(IReadOnlyList<GetLeagueTeamTransferBidsResult> bids, IReadOnlyList<GetLeaguePlayerTransferBidSwapPlayersResult> swapPlayers)
    {
        var swapPlayersByBid = swapPlayers.ToLookup(x => x.LeagueTeamPlayerTransferId);

        return new LeagueTeamTransferBidsDto
        {
            Pending = bids
                .Where(row => row.Status == TransferStatus.Pending)
                .Select(row => ToBidDto(row, swapPlayersByBid[row.Id].ToList()))
                .ToList(),
            Completed = bids
                .Where(row => row.Status is TransferStatus.Accepted or TransferStatus.Completed)
                .Select(row => ToBidDto(row, swapPlayersByBid[row.Id].ToList()))
                .ToList(),
        };
    }

    /// <summary>
    /// Converts a transfer bid row and the players offered in it into its DTO representation.
    /// </summary>
    /// <param name="row">The transfer bid row returned by the stored procedure.</param>
    /// <param name="swapPlayers">The offered-player rows of this bid, empty when it offers none.</param>
    /// <returns>The corresponding <see cref="LeagueTeamTransferBidDto"/>.</returns>
    public static LeagueTeamTransferBidDto ToBidDto(GetLeagueTeamTransferBidsResult row, IReadOnlyList<GetLeaguePlayerTransferBidSwapPlayersResult> swapPlayers)
    {
        return new LeagueTeamTransferBidDto
        {
            Id = row.Id,
            IsReceived = row.IsReceived,
            Status = row.Status,
            PlayerId = row.PlayerId,
            PlayerName = row.PlayerName,
            LeagueTeamId = row.LeagueTeamId,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            ManagerName = row.ManagerName,
            SeasonNumber = row.SeasonNumber,
            TransferFee = row.TransferFee,
            BuybackValue = row.BuybackValue,
            BuybackValidSeasons = row.BuybackValidSeasons,
            SwapPlayers = swapPlayers.Select(GetLeaguePlayerBidsMapper.ToSwapPlayerDto).ToList(),
            CreatedAt = row.CreatedAt,
        };
    }
}
