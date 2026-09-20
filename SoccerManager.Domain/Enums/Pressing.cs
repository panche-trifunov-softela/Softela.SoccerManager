namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how high up the pitch a team presses the opponent. The values deliberately start
/// at 1, so that an omitted or default zero fails <c>IsInEnum</c> validation rather than silently
/// meaning <see cref="HighPress"/>.
/// </summary>
public enum Pressing : byte
{
    /// <summary>
    /// The team presses high up the pitch.
    /// </summary>
    HighPress = 1,

    /// <summary>
    /// The team presses at a balanced height.
    /// </summary>
    Balanced = 2,

    /// <summary>
    /// The team sits back in a low block.
    /// </summary>
    LowBlock = 3,
}
