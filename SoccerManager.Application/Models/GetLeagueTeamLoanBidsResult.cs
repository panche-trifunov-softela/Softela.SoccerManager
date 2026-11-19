using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models;

/// <summary>
/// One row of the result set returned by the <c>dbo.GetLeagueTeamLoanBids</c> stored procedure: a pending, accepted or
/// completed loan bid a club placed or received, denormalised with the player, the other club's team and current manager and
/// the bid's season. Kept separate from <see cref="Dtos.LeagueTeamLoanBidDto"/> because Dapper populates a result by setting
/// properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeagueTeamLoanBidsResult
{
    /// <summary>
    /// The identifier of the loan bid.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Whether the bid was made for one of the club's players. When <see langword="false"/>, the club made the bid.
    /// </summary>
    public bool IsReceived { get; set; }

    /// <summary>
    /// Where the bid stands: <see cref="TransferStatus.Pending"/>, <see cref="TransferStatus.Accepted"/> or
    /// <see cref="TransferStatus.Completed"/>. A rejected bid is not returned.
    /// </summary>
    public TransferStatus Status { get; set; }

    /// <summary>
    /// The identifier of the player the bid is for.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// The name of the player the bid is for.
    /// </summary>
    public string PlayerName { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the player's photo, or <see langword="null"/> when he has none.
    /// </summary>
    public string? PlayerImageUrl { get; set; }

    /// <summary>
    /// The identifier of the other league team: the borrower for a received bid, the lending club for a placed one.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// The name of the other club.
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the other club's logo, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; set; }

    /// <summary>
    /// The full name of the other club's current manager, or <see langword="null"/> when the club has none or the manager has
    /// no name.
    /// </summary>
    public string? ManagerName { get; set; }

    /// <summary>
    /// The number of the bid's season within its league.
    /// </summary>
    public int SeasonNumber { get; set; }

    /// <summary>
    /// The fee the borrowing club offers for the loan. 0 means no fee.
    /// </summary>
    public decimal LoanFee { get; set; }

    /// <summary>
    /// The number of seasons the loan lasts, 1 or 2.
    /// </summary>
    public int SeasonsLength { get; set; }

    /// <summary>
    /// The price at which the loan club may buy the player when the loan ends, or <see langword="null"/> when the bid asks for
    /// no option to buy.
    /// </summary>
    public decimal? BuyOptionValue { get; set; }

    /// <summary>
    /// The UTC date and time the bid was placed.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
