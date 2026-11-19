using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamLoanBids;

/// <summary>
/// Maps a club's loan bid rows to a <see cref="LeagueTeamLoanBidsDto"/>.
/// </summary>
public static class GetLeagueTeamLoanBidsMapper
{
    /// <summary>
    /// Converts the loan bid rows into the bids' DTO representation: the pending bids in one list and the accepted and
    /// completed ones in the other.
    /// </summary>
    /// <param name="bids">The loan bid rows returned by the stored procedure, in the order to keep.</param>
    /// <returns>The corresponding <see cref="LeagueTeamLoanBidsDto"/>, with both lists in the order of their rows.</returns>
    public static LeagueTeamLoanBidsDto ToDto(IReadOnlyList<GetLeagueTeamLoanBidsResult> bids)
    {
        return new LeagueTeamLoanBidsDto
        {
            Pending = bids
                .Where(row => row.Status == TransferStatus.Pending)
                .Select(ToBidDto)
                .ToList(),
            Completed = bids
                .Where(row => row.Status is TransferStatus.Accepted or TransferStatus.Completed)
                .Select(ToBidDto)
                .ToList(),
        };
    }

    /// <summary>
    /// Converts a loan bid row into its DTO representation.
    /// </summary>
    /// <param name="row">The loan bid row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeagueTeamLoanBidDto"/>.</returns>
    public static LeagueTeamLoanBidDto ToBidDto(GetLeagueTeamLoanBidsResult row)
    {
        return new LeagueTeamLoanBidDto
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
            LoanFee = row.LoanFee,
            SeasonsLength = row.SeasonsLength,
            BuyOptionValue = row.BuyOptionValue,
            CreatedAt = row.CreatedAt,
        };
    }
}
