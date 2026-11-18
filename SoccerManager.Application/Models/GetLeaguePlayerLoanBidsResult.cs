namespace SoccerManager.Application.Models;

/// <summary>
/// One row of the fourth result set returned by the <c>dbo.GetLeaguePlayerBids</c> stored procedure: a pending loan bid on a
/// player of a club, denormalised with the bidding club's team and current manager and the bid's season. Kept separate from
/// <see cref="Dtos.LeaguePlayerLoanBidDto"/> because Dapper populates a result by setting properties, and the Dto is an
/// init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeaguePlayerLoanBidsResult
{
    /// <summary>
    /// The identifier of the loan bid.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The identifier of the bidding league team.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// The name of the bidding club.
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the bidding club's logo, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; set; }

    /// <summary>
    /// The full name of the bidding club's current manager, or <see langword="null"/> when the club has none or the manager
    /// has no name.
    /// </summary>
    public string? ManagerName { get; set; }

    /// <summary>
    /// The number of the bid's season within its league.
    /// </summary>
    public int SeasonNumber { get; set; }

    /// <summary>
    /// The fee the bidding club offers for the loan. 0 means no fee.
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
