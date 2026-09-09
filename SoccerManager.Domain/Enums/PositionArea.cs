namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the area of the pitch a position occupies. The values deliberately start at 1, so that
/// an omitted or default zero fails <c>IsInEnum</c> validation rather than silently meaning
/// <see cref="Attack"/>.
/// </summary>
public enum PositionArea : byte
{
    /// <summary>
    /// The position occupies the attack.
    /// </summary>
    Attack = 1,

    /// <summary>
    /// The position occupies the midfield.
    /// </summary>
    Midfield = 2,

    /// <summary>
    /// The position occupies the defence.
    /// </summary>
    Defence = 3,

    /// <summary>
    /// The position occupies the goalkeeper role.
    /// </summary>
    Goalkeeper = 4,
}
