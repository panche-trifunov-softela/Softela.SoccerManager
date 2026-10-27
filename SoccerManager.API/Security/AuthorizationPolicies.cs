namespace SoccerManager.API.Security;

/// <summary>
/// Names the authorization policies the API registers and the Keycloak realm roles they require.
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>
    /// The policy for endpoints only an operator may call.
    /// </summary>
    public const string Operator = "Operator";

    /// <summary>
    /// The Keycloak realm role that makes a user an operator.
    /// </summary>
    public const string OperatorRole = "admin";
}
