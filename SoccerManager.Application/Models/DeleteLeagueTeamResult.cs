namespace SoccerManager.Application.Models;

/// <summary>
/// The single row returned by the <c>dbo.DeleteLeagueTeam</c> stored procedure: whether the roster
/// entry was removed and, when it was not, which kinds of record still hang off the club in that league.
/// Kept as a settable class because Dapper populates a result by setting properties, and an init-only
/// record cannot be a Dapper target.
/// </summary>
public class DeleteLeagueTeamResult
{
    /// <summary>
    /// Whether the roster entry was removed. It is <see langword="false"/> when the entry does not
    /// exist or when anything is still attached to it.
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Whether the club still has squad rows in the league.
    /// </summary>
    public bool HasSquad { get; set; }

    /// <summary>
    /// Whether the club still has manager appointments in the league.
    /// </summary>
    public bool HasAppointments { get; set; }

    /// <summary>
    /// Whether the club still has manager applications in the league.
    /// </summary>
    public bool HasApplications { get; set; }

    /// <summary>
    /// Whether the club still has standings rows in the league.
    /// </summary>
    public bool HasStandings { get; set; }

    /// <summary>
    /// Whether the club still has match rows (statistics, tactics or formation positions) in the league.
    /// </summary>
    public bool HasMatches { get; set; }
}
