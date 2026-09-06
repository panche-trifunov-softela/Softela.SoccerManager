using System.Data.Common;
using System.Reflection;
using EvolveDb;
using SoccerManager.Infrastructure.Database.Connections;

namespace SoccerManager.Infrastructure.Database.Migrator;

/// <summary>
/// Applies pending database schema migrations using Evolve.
/// </summary>
public class DbMigrator : IDbMigrator
{
    private readonly IDatabaseConnection _databaseConnection;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbMigrator"/> class.
    /// </summary>
    /// <param name="databaseConnection">The provider used to obtain the database connection to migrate.</param>
    public DbMigrator(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    /// <summary>
    /// Applies every pending migration script under "Database/Scripts" to the database.
    /// </summary>
    public void Migrate()
    {
        var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var scriptsLocation = string.IsNullOrWhiteSpace(assemblyDirectory)
            ? Path.Combine("Database", "Scripts")
            : Path.Combine(assemblyDirectory, "Database", "Scripts");

        using var connection = _databaseConnection.GetConnection();

        // Erasing is disabled so a misconfigured run can never drop the schema.
        var evolve = new Evolve((DbConnection)connection)
        {
            IsEraseDisabled = true,
            CommandTimeout = 600,
            Locations = new[] { scriptsLocation },
        };

        evolve.Migrate();
    }
}
