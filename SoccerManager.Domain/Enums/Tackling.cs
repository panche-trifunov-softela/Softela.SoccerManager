namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how aggressively a team tackles. The values deliberately start at 1, so that an
/// omitted or default zero fails <c>IsInEnum</c> validation rather than silently meaning
/// <see cref="Soft"/>.
/// </summary>
public enum Tackling : byte
{
    /// <summary>
    /// The team tackles softly.
    /// </summary>
    Soft = 1,

    /// <summary>
    /// The team tackles at a normal intensity.
    /// </summary>
    Normal = 2,

    /// <summary>
    /// The team tackles hard.
    /// </summary>
    Hard = 3,
}
