using SoccerManager.Application.Core.User;

namespace SoccerManager.API.Security;

/// <summary>
/// Exposes the authenticated caller's identifier from the current HTTP request.
/// </summary>
public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUser"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The accessor used to reach the current HTTP context.</param>
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the identifier of the authenticated caller.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no authenticated user with a usable subject claim is available.</exception>
    public Guid UserId
    {
        get
        {
            var subject = _httpContextAccessor.HttpContext?.User.GetSubject();

            if (subject is not null && Guid.TryParse(subject, out var userId))
                return userId;

            throw new InvalidOperationException("No authenticated user is available.");
        }
    }
}
