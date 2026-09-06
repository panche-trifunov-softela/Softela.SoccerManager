using SoccerManager.API.Security;
using SoccerManager.Application.Core.User;

namespace SoccerManager.API.Extensions;

/// <summary>
/// Registers the current-user accessor.
/// </summary>
public static class CurrentUserExtensions
{
    /// <summary>
    /// Adds an HTTP-context-backed <see cref="ICurrentUser"/> implementation to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same service collection, for chaining.</returns>
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
