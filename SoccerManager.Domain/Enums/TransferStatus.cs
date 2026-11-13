namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents where a transfer or a loan of a player between two clubs of a league stands. The values deliberately
/// start at 1, so that a zero fails <c>IsInEnum</c> validation rather than silently meaning <see cref="Pending"/>.
/// <c>CK_LeagueTeamPlayerTransfers_StatusKnown</c> on <c>dbo.LeagueTeamPlayerTransfers.Status</c> and
/// <c>CK_LeagueTeamPlayerLoans_StatusKnown</c> on <c>dbo.LeagueTeamPlayerLoans.Status</c> accept exactly these values,
/// so a new member needs a new script as well.
/// </summary>
public enum TransferStatus : byte
{
    /// <summary>
    /// The transfer or loan has not been answered yet.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// The transfer or loan was agreed.
    /// </summary>
    Accepted = 2,

    /// <summary>
    /// The transfer or loan was turned down.
    /// </summary>
    Rejected = 3,
}
