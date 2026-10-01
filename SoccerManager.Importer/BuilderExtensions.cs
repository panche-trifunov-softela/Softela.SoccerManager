using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SoccerManager.Application;
using SoccerManager.Application.Core.User;
using SoccerManager.Importer.Database;
using SoccerManager.Importer.Database.Changes;
using SoccerManager.Importer.Database.Writing;
using SoccerManager.Importer.Dataset;
using SoccerManager.Importer.Import;
using SoccerManager.Importer.Report;
using SoccerManager.Importer.Scope;
using SoccerManager.Importer.Source;
using SoccerManager.Importer.Transform;
using SoccerManager.Importer.Wikidata;
using SoccerManager.Infrastructure;

namespace SoccerManager.Importer;

/// <summary>
/// Registers the importer's configuration and services with the dependency injection container.
/// </summary>
public static class BuilderExtensions
{
    /// <summary>
    /// Binds the Source, Scope, Wikidata, Transform and Import option sections and registers the importer's
    /// services, including the report writers. The application and infrastructure layers, and the fixed
    /// <see cref="ICurrentUser"/> every written row is stamped with, are only registered when
    /// ConnectionStrings:soccermanager is actually set, so an offline dry run never needs a database.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The configuration the importer's options bind to.</param>
    /// <returns>The same service collection, for chaining.</returns>
    public static IServiceCollection AddImporterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(BindOptions<SourceOptions>(configuration, SourceOptions.SectionName));
        services.AddSingleton(BindOptions<ScopeOptions>(configuration, ScopeOptions.SectionName));
        services.AddSingleton(BindOptions<WikidataOptions>(configuration, WikidataOptions.SectionName));
        services.AddSingleton(BindOptions<TransformOptions>(configuration, TransformOptions.SectionName));
        services.AddSingleton(BindOptions<RatingOptions>(configuration, RatingOptions.SectionName));
        services.AddSingleton(BindOptions<FinancialStateOptions>(configuration, FinancialStateOptions.SectionName));
        services.AddSingleton(BindOptions<WageOptions>(configuration, WageOptions.SectionName));
        services.AddSingleton(BindOptions<PositionsOptions>(configuration, PositionsOptions.SectionName));
        services.AddSingleton(BindOptions<ImagesOptions>(configuration, ImagesOptions.SectionName));
        services.AddSingleton(BindOptions<ImportOptions>(configuration, ImportOptions.SectionName));

        // Infrastructure's own DatabaseConnectionStringProvider rejects only a MISSING "soccermanager" key, reading
        // it eagerly at registration; an empty string does not throw there and would only fail later, at
        // SqlConnection.Open. Blank is treated the same as missing here, before any such late failure.
        var isConfigured = !string.IsNullOrWhiteSpace(configuration.GetConnectionString("soccermanager"));
        services.AddSingleton(new DatabaseSettings(isConfigured));

        if (isConfigured)
        {
            services.AddApplicationServices();
            services.AddInfrastructureServices(configuration);
            services.AddSingleton<ICurrentUser, ImportCurrentUser>();
        }

        // Registered unconditionally: each of these only resolves the application services above when it is
        // actually called, which the importer only does once the database is confirmed to be configured.
        services.AddSingleton<DatabaseSnapshotReader>();
        services.AddSingleton<ImportPlanner>();
        services.AddSingleton<ImportWriter>();
        services.AddSingleton<DatabaseChangesReport>();
        services.AddSingleton<WriteResultReport>();

        services.AddSingleton<DatasetStreamReader>();
        services.AddSingleton<DatasetLoader>();
        services.AddSingleton<WikidataClient>();
        services.AddSingleton<FinancialStateCalculator>();
        services.AddSingleton<WageEstimator>();
        services.AddSingleton<PlayerPositionBuilder>();
        services.AddSingleton<RatingCalculator>();
        services.AddSingleton<ImportModelBuilder>();
        services.AddSingleton<DryRunReport>();
        services.AddSingleton<ImportCommand>();

        return services;
    }

    // Binds a section by hand, into Options.Create, rather than through the Options.ConfigurationExtensions
    // package's Configure<T>(IConfiguration) helper: this project's only package references are Microsoft.Extensions.Hosting
    // and CsvHelper, and Configuration.Binder's Bind (already pulled in transitively by Hosting) is enough on its own.
    private static IOptions<TOptions> BindOptions<TOptions>(IConfiguration configuration, string sectionName)
        where TOptions : class, new()
    {
        var options = new TOptions();
        configuration.GetSection(sectionName).Bind(options);

        return Options.Create(options);
    }
}
