namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how a team approaches the match, from sitting deep to pushing forward. The values
/// deliberately start at 1, so that an omitted or default zero fails <c>IsInEnum</c> validation
/// rather than silently meaning <see cref="Defensive"/>.
/// </summary>
public enum Mentality : byte
{
    /// <summary>
    /// The team sits deep and prioritizes not conceding.
    /// </summary>
    Defensive = 1,

    /// <summary>
    /// The team balances defending and attacking.
    /// </summary>
    Balanced = 2,

    /// <summary>
    /// The team pushes forward and prioritizes scoring.
    /// </summary>
    Attacking = 3,
}
