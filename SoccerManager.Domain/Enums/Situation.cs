namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the state of the score a substitution is for, from the club's side. The values deliberately start at 1, so that
/// an omitted or default zero fails <c>IsInEnum</c> validation rather than silently meaning <see cref="Any"/>.
/// </summary>
public enum Situation : byte
{
    /// <summary>
    /// Whatever the score: the substitution used when the club has none for the actual state of the score.
    /// </summary>
    Any = 1,

    /// <summary>
    /// The club is ahead.
    /// </summary>
    Winning = 2,

    /// <summary>
    /// The score is level.
    /// </summary>
    Drawing = 3,

    /// <summary>
    /// The club is behind.
    /// </summary>
    Losing = 4,
}
