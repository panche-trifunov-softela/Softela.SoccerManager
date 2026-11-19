using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a transfer bid of a club, placed or received: the player, the other club and its current manager, the season,
/// the fee, the optional buyback and the players offered in a swap.
/// </summary>
public sealed record LeagueTeamTransferBidDto
{
    /// <summary>
    /// The identifier of the transfer bid. It is unique among transfers only: a loan bid can carry the same number.
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
    /// The URL of the player's photo, or <see langword="null"/> when he has none.
    /// </summary>
    public string? PlayerImageUrl { get; init; }

    /// <summary>
    /// The identifier of the other league team: the bidder for a received bid, the selling club for a placed one.
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
    /// The fee the bidding club offers, in euros. 0 means a free transfer.
    /// </summary>
    public decimal TransferFee { get; init; }

    /// <summary>
    /// The price at which the selling club could buy the player back, in euros, or <see langword="null"/> when the bid asks
    /// for no buyback.
    /// </summary>
    public decimal? BuybackValue { get; init; }

    /// <summary>
    /// The number of seasons the buyback stays valid, or <see langword="null"/> when the bid asks for no buyback.
    /// </summary>
    public int? BuybackValidSeasons { get; init; }

    /// <summary>
    /// The players the bidding club offers in a swap, in the order they were saved. Empty when the bid offers none.
    /// </summary>
    public required IReadOnlyList<LeaguePlayerBidSwapPlayerDto> SwapPlayers { get; init; }

    /// <summary>
    /// The UTC date and time the bid was placed.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
