namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how a finished match ended for one club. On a level score a shoot-out decides it.
/// </summary>
public enum MatchOutcome : byte
{
    /// <summary>
    /// The club scored more goals, or won the shoot-out after a level score.
    /// </summary>
    Win = 1,

    /// <summary>
    /// The score was level and no shoot-out decided it.
    /// </summary>
    Draw = 2,

    /// <summary>
    /// The club scored fewer goals, or lost the shoot-out after a level score.
    /// </summary>
    Loss = 3,
}
