using System.Security.Claims;

namespace SoccerManager.API.Security;

/// <summary>
/// Reads the Keycloak identity of a caller from its claims principal.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the subject of the caller's token, which is its Keycloak user id.
    /// </summary>
    /// <param name="principal">The principal to read.</param>
    /// <returns>The subject, or <see langword="null"/> when the principal carries none.</returns>
    public static string? GetSubject(this ClaimsPrincipal principal)
    {
        // MapInboundClaims is left at its default, so "sub" normally arrives as ClaimTypes.NameIdentifier;
        // the literal "sub" claim is checked too in case that mapping is ever turned off.
        return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? principal.FindFirst("sub")?.Value;
    }
}
