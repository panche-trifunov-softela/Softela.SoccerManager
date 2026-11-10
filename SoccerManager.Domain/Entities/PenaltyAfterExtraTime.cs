namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one kick of a penalty shootout after extra time.
/// </summary>
public class PenaltyAfterExtraTime : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the match the shootout belongs to.
    /// </summary>
    public int MatchId { get; set; }

    /// <summary>
    /// Gets or sets the place of the kick in the shootout, counted from 1. It is unique within the match.
    /// </summary>
    public int KickNumber { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the league roster entry (the club in its league) that took the kick.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player who took the kick.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets whether the kick scored.
    /// </summary>
    public bool IsScored { get; set; }
}
