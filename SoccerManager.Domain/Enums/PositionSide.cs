namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the side of the pitch a position occupies. The values deliberately start at 1, so that
/// an omitted or default zero fails <c>IsInEnum</c> validation rather than silently meaning
/// <see cref="Left"/>.
/// </summary>
public enum PositionSide : byte
{
    /// <summary>
    /// The position occupies the left side.
    /// </summary>
    Left = 1,

    /// <summary>
    /// The position occupies the center.
    /// </summary>
    Center = 2,

    /// <summary>
    /// The position occupies the right side.
    /// </summary>
    Right = 3,
}
