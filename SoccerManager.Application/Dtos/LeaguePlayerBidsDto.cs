namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents the pending bids on a player of a club: the transfer bids and the loan bids, each newest first.
/// </summary>
public sealed record LeaguePlayerBidsDto
{
    /// <summary>
    /// The pending transfer bids on the player, newest first.
    /// </summary>
    public required IReadOnlyList<LeaguePlayerTransferBidDto> TransferBids { get; init; }

    /// <summary>
    /// The pending loan bids on the player, newest first.
    /// </summary>
    public required IReadOnlyList<LeaguePlayerLoanBidDto> LoanBids { get; init; }
}
