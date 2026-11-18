namespace SoccerManager.Application.Models;

/// <summary>
/// The single row returned by the <c>dbo.DeleteNationalTeam</c> stored procedure: whether the national team was removed
/// and, when it was not, which kinds of record in any league still hang off its roster rows.
/// Kept as a settable class because Dapper populates a result by setting properties, and an init-only
/// record cannot be a Dapper target.
/// </summary>
public class DeleteNationalTeamResult
{
    /// <summary>
    /// Whether the national team was removed. It is <see langword="false"/> when the national team does not exist or
    /// when a league still has anything attached to it.
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Whether the national team still has standings rows in any league.
    /// </summary>
    public bool HasStandings { get; set; }

    /// <summary>
    /// Whether the national team still has match rows in any league: team statistics, tactics, formation positions,
    /// substitutions, player statistics, shootout kicks or absences.
    /// </summary>
    public bool HasMatches { get; set; }

    /// <summary>
    /// Whether the national team still has a transfer from or to one of its roster rows, a loan to one, or a swap naming
    /// one of its squad rows, in any league.
    /// </summary>
    public bool HasTransfers { get; set; }
}
