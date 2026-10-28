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

    /// <summary>
    /// Gets a value indicating whether the authenticated caller is an operator, who may act on every club.
    /// </summary>
    bool IsOperator { get; }
}
