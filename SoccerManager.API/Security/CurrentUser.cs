using System.Security.Claims;
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
            var user = _httpContextAccessor.HttpContext?.User;

            // MapInboundClaims is left at its default, so "sub" normally arrives as ClaimTypes.NameIdentifier;
            // the literal "sub" claim is checked too in case that mapping is ever turned off.
            var subject = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user?.FindFirst("sub")?.Value;

            if (subject is not null && Guid.TryParse(subject, out var userId))
                return userId;

            throw new InvalidOperationException("No authenticated user is available.");
        }
    }
}
