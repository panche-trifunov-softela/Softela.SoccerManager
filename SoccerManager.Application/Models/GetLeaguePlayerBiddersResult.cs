using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models;

/// <summary>
/// One row of the second result set returned by the <c>dbo.GetLeaguePlayerBidders</c> stored procedure: a pending or accepted
/// (not yet completed) transfer or loan bid on a player of a league, denormalised with the bidding club's team and current
/// manager and the bid's season, without any term of the bid. Kept separate from <see cref="Dtos.LeaguePlayerBidderDto"/>
/// because Dapper populates a result by setting properties, and the Dto is an init-only record that cannot be a Dapper
/// target.
/// </summary>
public class GetLeaguePlayerBiddersResult
{
    /// <summary>
    /// The identifier of the transfer or, for a loan, of the loan. The two tables count separately, so it is only unique
    /// together with <see cref="IsLoan"/>.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Whether the bid is for a loan rather than a transfer.
    /// </summary>
    public bool IsLoan { get; set; }

    /// <summary>
    /// Where the bid stands: <see cref="TransferStatus.Pending"/>, or <see cref="TransferStatus.Accepted"/> for a deal agreed
    /// but not carried out yet. A completed deal (<see cref="TransferStatus.Completed"/>) is not listed.
    /// </summary>
    public TransferStatus Status { get; set; }

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
    /// The UTC date and time the bid was placed.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
