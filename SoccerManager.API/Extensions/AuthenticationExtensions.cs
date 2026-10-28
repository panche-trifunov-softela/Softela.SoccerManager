using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SoccerManager.API.Security;

namespace SoccerManager.API.Extensions;

/// <summary>
/// Registers JWT bearer authentication against a Keycloak realm, and the authorization policies built on its roles.
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Configures JWT bearer authentication and authorization using Keycloak settings from configuration.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The configuration containing the Keycloak settings.</param>
    /// <returns>The same service collection, for chaining.</returns>
    public static IServiceCollection AddKeycloakAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Keycloak:Authority"];
                options.Audience = configuration["Keycloak:Audience"];
                options.RequireHttpsMetadata = configuration.GetValue("Keycloak:RequireHttpsMetadata", true);

                // Keycloak signs with RS256, so a token signed with any other algorithm is refused.
                options.TokenValidationParameters.ValidAlgorithms = [SecurityAlgorithms.RsaSha256];

                // An expired token gets 30 seconds of grace for clock drift, not the 5-minute default.
                options.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(30);

                // Keycloak nests realm roles inside realm_access, which the handler never maps to role claims.
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (context.Principal is not null)
                            KeycloakRoleClaims.AddRealmRoles(context.Principal);

                        return Task.CompletedTask;
                    },
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.Operator, policy => policy.RequireRole(AuthorizationPolicies.OperatorRole));
        });

        return services;
    }
}
