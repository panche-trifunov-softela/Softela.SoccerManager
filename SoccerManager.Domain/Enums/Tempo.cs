namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the speed at which a team moves the ball and plays. The values deliberately start
/// at 1, so that an omitted or default zero fails <c>IsInEnum</c> validation rather than silently
/// meaning <see cref="Slow"/>.
/// </summary>
public enum Tempo : byte
{
    /// <summary>
    /// The team moves the ball slowly and patiently.
    /// </summary>
    Slow = 1,

    /// <summary>
    /// The team moves the ball at a normal pace.
    /// </summary>
    Normal = 2,

    /// <summary>
    /// The team moves the ball quickly.
    /// </summary>
    Fast = 3,
}
