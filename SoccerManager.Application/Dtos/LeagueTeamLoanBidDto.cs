using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a loan bid of a club, placed or received: the player, the other club and its current manager, the season, the
/// fee, the length of the loan and the optional option to buy.
/// </summary>
public sealed record LeagueTeamLoanBidDto
{
    /// <summary>
    /// The identifier of the loan bid. It is unique among loans only: a transfer bid can carry the same number.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Whether the bid was made for one of the club's players. When <see langword="false"/>, the club made the bid.
    /// </summary>
    public bool IsReceived { get; init; }

    /// <summary>
    /// Where the bid stands: <see cref="TransferStatus.Pending"/>, <see cref="TransferStatus.Accepted"/> for a deal agreed but
    /// not carried out yet, or <see cref="TransferStatus.Completed"/>.
    /// </summary>
    public TransferStatus Status { get; init; }

    /// <summary>
    /// The identifier of the player the bid is for.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The name of the player the bid is for.
    /// </summary>
    public required string PlayerName { get; init; }

    /// <summary>
    /// The identifier of the other league team: the borrower for a received bid, the lending club for a placed one.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The name of the other club.
    /// </summary>
    public required string TeamName { get; init; }

    /// <summary>
    /// The URL of the other club's logo, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; init; }

    /// <summary>
    /// The full name of the other club's current manager, or <see langword="null"/> when the club has none or the manager has
    /// no name.
    /// </summary>
    public string? ManagerName { get; init; }

    /// <summary>
    /// The number of the season the bid was placed in, within its league.
    /// </summary>
    public int SeasonNumber { get; init; }

    /// <summary>
    /// The fee the borrowing club offers for the loan, in euros. 0 means no fee.
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
