namespace SoccerManager.Infrastructure.Database.Migrator;

/// <summary>
/// Applies pending database schema migrations.
/// </summary>
public interface IDbMigrator
{
    /// <summary>
    /// Applies every pending migration script to the database.
    /// </summary>
    void Migrate();
}
