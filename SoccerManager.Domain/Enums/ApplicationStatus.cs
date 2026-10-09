namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents where a manager's application to manage a team in a league stands.
/// The values deliberately start at 1, so that a zero fails <c>IsInEnum</c> validation rather
/// than silently meaning <see cref="Pending"/>; an application created without one is
/// <see cref="Pending"/>.
/// </summary>
public enum ApplicationStatus : byte
{
    /// <summary>
    /// The application has not been answered yet.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// The application was accepted, and the manager was appointed to the team.
    /// </summary>
    Accepted = 2,

    /// <summary>
    /// The application was rejected, either directly or because another acceptance made it moot.
    /// </summary>
    Rejected = 3,
}
