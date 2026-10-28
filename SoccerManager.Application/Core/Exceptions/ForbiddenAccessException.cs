namespace SoccerManager.Application.Core.Exceptions;

/// <summary>
/// Raised when the authenticated caller is not allowed to perform the requested action, and translated into
/// a 403 response by the API.
/// </summary>
public sealed class ForbiddenAccessException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenAccessException"/> class.
    /// </summary>
    /// <param name="message">The reason the caller is not allowed, safe to show to the caller.</param>
    public ForbiddenAccessException(string message)
        : base(message)
    {
    }
}
