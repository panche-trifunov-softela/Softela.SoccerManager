namespace SoccerManager.API.Extensions;

/// <summary>
/// Registers a configurable CORS policy.
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Adds the default CORS policy using the allowed origins from configuration.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The configuration containing the allowed origins.</param>
    /// <returns>The same service collection, for chaining.</returns>
    public static IServiceCollection AddConfiguredCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}
