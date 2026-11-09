using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents the tactic a club switches to at one minute of a match when the score is in one situation: the same
/// instructions and roles as its main tactic, for that substitution.
/// </summary>
public class MatchTeamTacticSubstitution : MatchTeamTactic
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
