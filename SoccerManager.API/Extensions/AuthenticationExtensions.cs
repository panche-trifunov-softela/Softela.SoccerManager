using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SoccerManager.API.Extensions;

/// <summary>
/// Registers JWT bearer authentication against a Keycloak realm.
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
                options.RequireHttpsMetadata = configuration.GetValue<bool>("Keycloak:RequireHttpsMetadata");
            });

        services.AddAuthorization();

        return services;
    }
}
