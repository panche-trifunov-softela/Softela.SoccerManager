using SoccerManager.Application.Core.User;

namespace SoccerManager.API.Security;

/// <summary>
/// Exposes the authenticated caller's identifier and name from the current HTTP request.
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

    /// <summary>
    /// Gets a value indicating whether the authenticated caller holds the operator realm role.
    /// </summary>
    /// <remarks>
    /// <see langword="false"/> when there is no HTTP context.
    /// </remarks>
    public bool IsOperator => _httpContextAccessor.HttpContext?.User.IsInRole(AuthorizationPolicies.OperatorRole) ?? false;

    /// <summary>
    /// Gets the authenticated caller's first name from their token.
    /// </summary>
    /// <remarks>
    /// <see langword="null"/> when there is no HTTP context or the token carries no such claim.
    /// </remarks>
    public string? FirstName => _httpContextAccessor.HttpContext?.User.GetGivenName();

    /// <summary>
    /// Gets the authenticated caller's last name from their token.
    /// </summary>
    /// <remarks>
    /// <see langword="null"/> when there is no HTTP context or the token carries no such claim.
    /// </remarks>
    public string? LastName => _httpContextAccessor.HttpContext?.User.GetFamilyName();
}
