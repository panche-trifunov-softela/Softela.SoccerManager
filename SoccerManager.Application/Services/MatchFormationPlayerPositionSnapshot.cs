namespace SoccerManager.Application.Services;

/// <summary>
/// Carries the values a match formation player position copies from the player's league registration and
/// position rating at write time.
/// </summary>
public sealed record MatchFormationPlayerPositionSnapshot
{
    /// <summary>
    /// The player's condition at the time of the match, from 1 to 100.
    /// </summary>
    public int ConditionOnMatch { get; init; }

    /// <summary>
    /// The player's quality in the position at the time of the match, from 1 to 100.
    /// </summary>
    public int QualityAtPositionOnMatch { get; init; }

    /// <summary>
    /// Whether the player is suspended for the match.
    /// </summary>
    public bool IsSuspended { get; init; }
}
