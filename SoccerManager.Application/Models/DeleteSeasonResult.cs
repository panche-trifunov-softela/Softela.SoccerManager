namespace SoccerManager.Application.Models;

/// <summary>
/// The single row returned by the <c>dbo.DeleteSeason</c> stored procedure: whether the season was
/// removed and, when it was not, whether transfers or loans still use it.
/// Kept as a settable class because Dapper populates a result by setting properties, and an init-only
/// record cannot be a Dapper target.
/// </summary>
public class DeleteSeasonResult
{
    /// <summary>
    /// Whether the season was removed. It is <see langword="false"/> when the season does not exist or
    /// when a transfer or loan still uses it.
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Whether a transfer or loan still uses the season.
    /// </summary>
    public bool HasTransfers { get; set; }
}
