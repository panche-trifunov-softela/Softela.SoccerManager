using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetMatchLineupSubstitutionRows</c> stored procedure: a stored slot of one of a club's
/// substitution lineups for a match, with the columns of <see cref="GetMatchLineupRowsResult"/> plus the substitution's minute
/// and situation. Dapper populates a result by setting properties, so the row is its own class rather than a domain entity.
/// </summary>
public class GetMatchLineupSubstitutionRowsResult : GetMatchLineupRowsResult
{
    /// <summary>
    /// The minute of the substitution the row belongs to.
    /// </summary>
    public int MinuteAt { get; set; }

    /// <summary>
    /// The situation of the substitution the row belongs to.
    /// </summary>
    public Situation Situation { get; set; }
}
