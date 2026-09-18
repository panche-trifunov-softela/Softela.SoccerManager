namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how a player feels about their situation at the team. The values deliberately start
/// at 1, so that an omitted or default zero fails <c>IsInEnum</c> validation rather than silently
/// meaning <see cref="VeryConcerned"/>.
/// </summary>
public enum Morale : byte
{
    /// <summary>
    /// The player is very concerned.
    /// </summary>
    VeryConcerned = 1,

    /// <summary>
    /// The player is concerned.
    /// </summary>
    Concerned = 2,

    /// <summary>
    /// The player's morale is normal.
    /// </summary>
    Normal = 3,

    /// <summary>
    /// The player is satisfied.
    /// </summary>
    Satisfied = 4,

    /// <summary>
    /// The player is very satisfied.
    /// </summary>
    VerySatisfied = 5,
}
