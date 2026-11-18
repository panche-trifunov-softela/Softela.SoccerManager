namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a pending loan bid on a player of a club: the bidding club and its current manager, the season, the fee, the
/// length of the loan and the optional option to buy.
/// </summary>
public sealed record LeaguePlayerLoanBidDto
{
    /// <summary>
    /// The identifier of the loan bid. It is unique among loans only: a transfer bid can carry the same number.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the bidding league team.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The name of the bidding club.
    /// </summary>
    public required string TeamName { get; init; }

    /// <summary>
    /// The URL of the bidding club's logo, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; init; }

    /// <summary>
    /// The full name of the bidding club's current manager, or <see langword="null"/> when the club has none or the manager
    /// has no name.
    /// </summary>
    public string? ManagerName { get; init; }

    /// <summary>
    /// The number of the bid's season within its league.
    /// </summary>
    public int SeasonNumber { get; init; }

    /// <summary>
    /// The fee the bidding club offers for the loan, in euros. 0 means no fee.
    /// </summary>
    public decimal LoanFee { get; init; }

    /// <summary>
    /// The number of seasons the loan lasts, 1 or 2.
    /// </summary>
    public int SeasonsLength { get; init; }

    /// <summary>
    /// The price at which the loan club may buy the player when the loan ends, in euros, or <see langword="null"/> when the
    /// bid asks for no option to buy.
    /// </summary>
    public decimal? BuyOptionValue { get; init; }

    /// <summary>
    /// The UTC date and time the bid was placed.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
