using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a club's bid for a player of a league: the bidding club and its current manager, the season, the day, whether
/// it is a transfer or a loan and whether it is pending or accepted. It carries no term of the bid, because any manager of the
/// league may read it.
/// </summary>
public sealed record LeaguePlayerBidderDto
{
    /// <summary>
    /// The identifier of the bid, from the transfers or the loans table according to <see cref="IsLoan"/>, so
    /// <see cref="IsLoan"/> together with the identifier is the key.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Whether the bid is for a loan rather than a transfer.
    /// </summary>
    public bool IsLoan { get; init; }

    /// <summary>
    /// Where the bid stands: <see cref="TransferStatus.Pending"/>, or <see cref="TransferStatus.Accepted"/> for a deal agreed
    /// but not completed yet.
    /// </summary>
    public TransferStatus Status { get; init; }

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
    /// The number of the season the bid was placed in, within its league.
    /// </summary>
    public int SeasonNumber { get; init; }

    /// <summary>
    /// The UTC date and time the bid was placed.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
