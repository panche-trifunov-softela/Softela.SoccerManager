using Microsoft.Extensions.Configuration;

namespace SoccerManager.Infrastructure.Database.Connections;

/// <summary>
/// Resolves the database connection string from application configuration.
/// </summary>
public class DatabaseConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConnectionStringProvider"/> class.
    /// </summary>
    /// <param name="configuration">The configuration containing the connection string.</param>
    public DatabaseConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Retrieves the configured "soccermanager" connection string.
    /// </summary>
    /// <returns>The connection string used to connect to the database.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the "soccermanager" connection string is not configured.</exception>
    public string GetConnectionString()
    {
        return _configuration.GetConnectionString("soccermanager")
            ?? throw new InvalidOperationException("Connection string 'soccermanager' not found.");
    }
}
