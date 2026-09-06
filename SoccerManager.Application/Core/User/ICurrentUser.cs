namespace SoccerManager.Application.Core.User;

/// <summary>
/// Exposes the authenticated caller's identifier to the application layer.
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Gets the identifier of the authenticated caller.
    /// </summary>
    Guid UserId { get; }
}
