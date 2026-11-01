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

    /// <summary>
    /// Gets the given name of the caller's token, which is their Keycloak first name.
    /// </summary>
    /// <param name="principal">The principal to read.</param>
    /// <returns>The given name, or <see langword="null"/> when the principal carries none.</returns>
    public static string? GetGivenName(this ClaimsPrincipal principal)
    {
        // MapInboundClaims is left at its default, so "given_name" normally arrives as ClaimTypes.GivenName;
        // the literal "given_name" claim is checked too in case that mapping is ever turned off.
        return principal.FindFirst(ClaimTypes.GivenName)?.Value
            ?? principal.FindFirst("given_name")?.Value;
    }

    /// <summary>
    /// Gets the family name of the caller's token, which is their Keycloak last name.
    /// </summary>
    /// <param name="principal">The principal to read.</param>
    /// <returns>The family name, or <see langword="null"/> when the principal carries none.</returns>
    public static string? GetFamilyName(this ClaimsPrincipal principal)
    {
        // MapInboundClaims is left at its default, so "family_name" normally arrives as ClaimTypes.Surname;
        // the literal "family_name" claim is checked too in case that mapping is ever turned off.
        return principal.FindFirst(ClaimTypes.Surname)?.Value
            ?? principal.FindFirst("family_name")?.Value;
    }
}
