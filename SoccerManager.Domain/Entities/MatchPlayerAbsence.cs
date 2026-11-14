using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one player's absence from one match: the club the player was missing from and why.
/// </summary>
public class MatchPlayerAbsence : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the match the player missed.
    /// </summary>
    public int MatchId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the league team (the club in its league) the player was missing from.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player who missed the match.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets why the player missed the match.
    /// </summary>
    public MatchAbsenceReason Reason { get; set; }
}
