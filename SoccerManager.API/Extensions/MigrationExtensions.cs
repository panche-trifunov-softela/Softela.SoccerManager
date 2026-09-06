using SoccerManager.Infrastructure.Database.Migrator;

namespace SoccerManager.API.Extensions;

/// <summary>
/// Applies pending database migrations during application startup.
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Runs the pending migrations when <c>Database:RunMigrationsOnStartup</c> is enabled, and does nothing otherwise.
    /// </summary>
    /// <param name="app">The application whose configuration and services are used.</param>
    /// <returns>The same application, for chaining.</returns>
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        // Off by default outside Development, so a deployment applies schema changes as its own
        // step rather than implicitly on every replica start.
        if (!app.Configuration.GetValue<bool>("Database:RunMigrationsOnStartup"))
            return app;

        using var scope = app.Services.CreateScope();

        var migrator = scope.ServiceProvider.GetRequiredService<IDbMigrator>();
        migrator.Migrate();

        return app;
    }
}
