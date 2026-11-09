using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one slot of the lineup a club switches to at one minute of a match when the score is in one situation,
/// snapshotted like a main lineup slot.
/// </summary>
public class MatchFormationPlayerPositionSubstitution : MatchFormationPlayerPosition
{
    /// <summary>
    /// Gets or sets the minute of the match the substitution takes effect at: 45, 60 or 75.
    /// </summary>
    public int MinuteAt { get; set; }

    /// <summary>
    /// Gets or sets the state of the score the substitution is for.
    /// </summary>
    public Situation Situation { get; set; }
}
