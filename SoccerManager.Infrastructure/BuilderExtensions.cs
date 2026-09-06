using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoccerManager.Application.Repositories;
using SoccerManager.Infrastructure.Repositories;

namespace SoccerManager.Infrastructure;

/// <summary>
/// Registers the infrastructure layer's service implementations.
/// </summary>
public static class BuilderExtensions
{
    /// <summary>
    /// Adds the infrastructure layer's service implementations to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The configuration the infrastructure layer reads its settings from.</param>
    /// <returns>The same service collection, for chaining.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // configuration is unused for now; it is the seam real persistence will read its connection string from.
        services.AddSingleton<ILeagueRepository, InMemoryLeagueRepository>();

        return services;
    }
}
