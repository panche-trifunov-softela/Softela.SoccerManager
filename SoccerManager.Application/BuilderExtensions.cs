using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SoccerManager.Application.Core.Behaviors;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
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

        // Registered after the validators so the behaviour resolves the ones scanned above.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
