namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one slot of a team's lineup for a match: the player position rating that fills a
/// formation slot, together with the player's condition, quality and suspension status snapshotted
/// for that match.
/// </summary>
public class MatchFormationPlayerPosition : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the match the lineup is for.
    /// </summary>
    public int MatchId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the team lining up.
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the formation slot being filled.
    /// </summary>
    public int FormationPositionId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player position rating filling the slot.
    /// </summary>
    public int PlayerPositionId { get; set; }

    /// <summary>
    /// Gets or sets the player's condition at the time of the match, from 1 to 100, copied from
    /// the league registration.
    /// </summary>
    public int ConditionOnMatch { get; set; }

    /// <summary>
    /// Gets or sets the player's quality in the position at the time of the match, from 1 to 100,
    /// copied from the position rating.
    /// </summary>
    public int QualityAtPositionOnMatch { get; set; }

    /// <summary>
    /// Gets or sets whether the player is suspended for the match, copied from the league
    /// registration.
    /// </summary>
    public bool IsSuspended { get; set; }
}
