namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the area of the pitch a team directs its attacks through. The values deliberately
/// start at 1, so that an omitted or default zero fails <c>IsInEnum</c> validation rather than
/// silently meaning <see cref="Mixed"/>.
/// </summary>
public enum AttackingSide : byte
{
    /// <summary>
    /// The team mixes its attacks across the pitch.
    /// </summary>
    Mixed = 1,

    /// <summary>
    /// The team attacks through the middle.
    /// </summary>
    ThroughTheMiddle = 2,

    /// <summary>
    /// The team attacks through both flanks.
    /// </summary>
    BothFlanks = 3,

    /// <summary>
    /// The team attacks through the left flank.
    /// </summary>
    LeftFlank = 4,

    /// <summary>
    /// The team attacks through the right flank.
    /// </summary>
    RightFlank = 5,
}
