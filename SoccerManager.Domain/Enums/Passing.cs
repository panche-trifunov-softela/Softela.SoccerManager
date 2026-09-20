namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the passing style a team favours. The values deliberately start at 1, so that an
/// omitted or default zero fails <c>IsInEnum</c> validation rather than silently meaning
/// <see cref="Short"/>.
/// </summary>
public enum Passing : byte
{
    /// <summary>
    /// The team favours short passes.
    /// </summary>
    Short = 1,

    /// <summary>
    /// The team favours direct passes.
    /// </summary>
    Direct = 2,

    /// <summary>
    /// The team favours long passes.
    /// </summary>
    Long = 3,

    /// <summary>
    /// The team mixes short, direct and long passes.
    /// </summary>
    Mixed = 4,
}
