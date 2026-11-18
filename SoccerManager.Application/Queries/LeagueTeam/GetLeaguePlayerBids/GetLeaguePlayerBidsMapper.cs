using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBids;

/// <summary>
/// Maps a player's pending bid rows to a <see cref="LeaguePlayerBidsDto"/>.
/// </summary>
public static class GetLeaguePlayerBidsMapper
{
    /// <summary>
    /// Converts the transfer bid rows, their swapped-player rows and the loan bid rows into the bids' DTO representation, each
    /// transfer bid carrying its own offered players in row order.
    /// </summary>
    /// <param name="transferBids">The transfer bid rows returned by the stored procedure, in the order to keep.</param>
    /// <param name="swapPlayers">The offered-player rows returned by the stored procedure, in swap-row order.</param>
    /// <param name="loanBids">The loan bid rows returned by the stored procedure, in the order to keep.</param>
    /// <returns>The corresponding <see cref="LeaguePlayerBidsDto"/>, with both lists in the order of their rows.</returns>
    public static LeaguePlayerBidsDto ToDto(IReadOnlyList<GetLeaguePlayerTransferBidsResult> transferBids, IReadOnlyList<GetLeaguePlayerTransferBidSwapPlayersResult> swapPlayers, IReadOnlyList<GetLeaguePlayerLoanBidsResult> loanBids)
    {
        var swapPlayersByBid = swapPlayers.ToLookup(x => x.LeagueTeamPlayerTransferId);

        return new LeaguePlayerBidsDto
        {
            TransferBids = transferBids.Select(row => ToTransferBidDto(row, swapPlayersByBid[row.Id].ToList())).ToList(),
            LoanBids = loanBids.Select(ToLoanBidDto).ToList(),
        };
    }

    /// <summary>
    /// Converts a transfer bid row and the players offered in it into its DTO representation.
    /// </summary>
    /// <param name="row">The transfer bid row returned by the stored procedure.</param>
    /// <param name="swapPlayers">The offered-player rows of this bid, empty when it offers none.</param>
    /// <returns>The corresponding <see cref="LeaguePlayerTransferBidDto"/>.</returns>
    public static LeaguePlayerTransferBidDto ToTransferBidDto(GetLeaguePlayerTransferBidsResult row, IReadOnlyList<GetLeaguePlayerTransferBidSwapPlayersResult> swapPlayers)
    {
        return new LeaguePlayerTransferBidDto
        {
            Id = row.Id,
            LeagueTeamId = row.LeagueTeamId,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            ManagerName = row.ManagerName,
            SeasonNumber = row.SeasonNumber,
            TransferFee = row.TransferFee,
            BuybackValue = row.BuybackValue,
            BuybackValidSeasons = row.BuybackValidSeasons,
            SwapPlayers = swapPlayers.Select(ToSwapPlayerDto).ToList(),
            CreatedAt = row.CreatedAt,
        };
    }

    /// <summary>
    /// Converts an offered-player row into its DTO representation.
    /// </summary>
    /// <param name="row">The offered-player row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeaguePlayerBidSwapPlayerDto"/>.</returns>
    public static LeaguePlayerBidSwapPlayerDto ToSwapPlayerDto(GetLeaguePlayerTransferBidSwapPlayersResult row)
    {
        return new LeaguePlayerBidSwapPlayerDto
        {
            PlayerId = row.PlayerId,
            Name = row.Name,
            PositionShortName = row.PositionShortName,
            Rating = row.Rating,
        };
    }

    /// <summary>
    /// Converts a loan bid row into its DTO representation.
    /// </summary>
    /// <param name="row">The loan bid row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeaguePlayerLoanBidDto"/>.</returns>
    public static LeaguePlayerLoanBidDto ToLoanBidDto(GetLeaguePlayerLoanBidsResult row)
    {
        return new LeaguePlayerLoanBidDto
        {
            Id = row.Id,
            LeagueTeamId = row.LeagueTeamId,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            ManagerName = row.ManagerName,
            SeasonNumber = row.SeasonNumber,
            LoanFee = row.LoanFee,
            SeasonsLength = row.SeasonsLength,
            BuyOptionValue = row.BuyOptionValue,
            CreatedAt = row.CreatedAt,
        };
    }
}
