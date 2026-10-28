using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SoccerManager.Application.Core.Behaviors;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Services;
using System.Reflection;

namespace SoccerManager.Application;

/// <summary>
/// Registers the application layer's request pipeline with the dependency injection container.
/// </summary>
public static class BuilderExtensions
{
    private static Assembly ApplicationAssembly => typeof(BuilderExtensions).Assembly;

    /// <summary>
    /// Registers MediatR, the command and query dispatchers, and validation for every handler in this assembly.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same service collection, for chaining.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(ApplicationAssembly))
            .AddScoped<ICommandDispatcher, CommandDispatcher>()
            .AddScoped<IQueryDispatcher, QueryDispatcher>();

        services.AddValidatorsFromAssembly(ApplicationAssembly);

        // The application services beyond the dispatchers: each reads several aggregates for a rule more than one
        // handler needs, so it lives outside any single handler. The resolver snapshots a lineup slot, the rules
        // guard it for every caller, and club access decides whether the caller may change a club at all.
        services.AddScoped<IMatchFormationPlayerPositionSnapshotResolver, MatchFormationPlayerPositionSnapshotResolver>();
        services.AddScoped<IMatchFormationPlayerPositionRules, MatchFormationPlayerPositionRules>();
        services.AddScoped<IClubAccess, ClubAccess>();

        // Registered after the validators so the behaviour resolves the ones scanned above.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
