namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how wide a team spreads its play across the pitch. The values deliberately start
/// at 1, so that an omitted or default zero fails <c>IsInEnum</c> validation rather than silently
/// meaning <see cref="Narrow"/>.
/// </summary>
public enum Width : byte
{
    /// <summary>
    /// The team keeps its play narrow.
    /// </summary>
    Narrow = 1,

    /// <summary>
    /// The team spreads its play at a balanced width.
    /// </summary>
    Balanced = 2,

    /// <summary>
    /// The team spreads its play wide.
    /// </summary>
    Wide = 3,
}
