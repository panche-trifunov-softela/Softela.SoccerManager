using System.Security.Claims;
using System.Text.Json;

namespace SoccerManager.API.Security;

/// <summary>
/// Copies the realm roles Keycloak nests in a token's <c>realm_access</c> claim onto the caller's identity as role claims.
/// </summary>
public static class KeycloakRoleClaims
{
    private const string RealmAccessClaimType = "realm_access";

    private const string RolesPropertyName = "roles";

    /// <summary>
    /// Adds one role claim per realm role in the principal's <c>realm_access</c> claim. A missing or malformed
    /// claim adds none, so the caller ends up holding no role rather than an unchecked one.
    /// </summary>
    /// <param name="principal">The principal built from a validated access token.</param>
    public static void AddRealmRoles(ClaimsPrincipal principal)
    {
        if (principal.Identity is not ClaimsIdentity identity)
            return;

        var realmAccess = principal.FindFirst(RealmAccessClaimType)?.Value;

        if (string.IsNullOrWhiteSpace(realmAccess))
            return;

        foreach (var role in ReadRoles(realmAccess))
            identity.AddClaim(new Claim(identity.RoleClaimType, role));
    }

    /// <summary>
    /// Reads the role names from the JSON value of a <c>realm_access</c> claim.
    /// </summary>
    /// <param name="realmAccess">The claim's JSON value.</param>
    /// <returns>The role names, or none when the value does not have the expected shape.</returns>
    private static IReadOnlyList<string> ReadRoles(string realmAccess)
    {
        try
        {
            using var document = JsonDocument.Parse(realmAccess);

            var root = document.RootElement;

            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty(RolesPropertyName, out var roles))
                return [];

            if (roles.ValueKind != JsonValueKind.Array)
                return [];

            return roles.EnumerateArray()
                .Where(role => role.ValueKind == JsonValueKind.String)
                .Select(role => role.GetString()!)
                .ToList();
        }
        catch (JsonException)
        {
            return [];
        }
    }
}
