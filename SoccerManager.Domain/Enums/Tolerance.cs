namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how much foul play a referee lets go before penalizing it, from strict to lenient.
/// The values deliberately start at 1, so that a zero fails <c>IsInEnum</c> validation rather
/// than silently meaning <see cref="Low"/>; a referee created without one gets
/// <see cref="Balanced"/>.
/// </summary>
public enum Tolerance : byte
{
    /// <summary>
    /// The referee is strict and penalizes foul play readily.
    /// </summary>
    Low = 1,

    /// <summary>
    /// The referee weighs penalizing foul play against letting the game flow.
    /// </summary>
    Balanced = 2,

    /// <summary>
    /// The referee is lenient and lets the game flow, penalizing only clear foul play.
    /// </summary>
    High = 3,
}
