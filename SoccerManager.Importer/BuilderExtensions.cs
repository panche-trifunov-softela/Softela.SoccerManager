using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SoccerManager.Importer.Dataset;
using SoccerManager.Importer.Import;
using SoccerManager.Importer.Report;
using SoccerManager.Importer.Scope;
using SoccerManager.Importer.Source;
using SoccerManager.Importer.Transform;
using SoccerManager.Importer.Wikidata;

namespace SoccerManager.Importer;

/// <summary>
/// Registers the importer's configuration and services with the dependency injection container.
/// </summary>
public static class BuilderExtensions
{
    /// <summary>
    /// Binds the Source, Scope, Wikidata and Transform option sections and registers the importer's services,
    /// including the dry-run report writer.
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
