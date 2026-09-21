using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoccerManager.Application.Repositories;
using SoccerManager.Infrastructure.Database;
using SoccerManager.Infrastructure.Database.Connections;
using SoccerManager.Infrastructure.Database.Dapper;
using SoccerManager.Infrastructure.Database.Migrator;
using SoccerManager.Infrastructure.Database.Repositories;

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
    /// <exception cref="InvalidOperationException">Thrown when the 'soccermanager' connection string is missing.</exception>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Resolved here rather than lazily so a missing connection string fails at startup, not at the first request.
        var connectionString = new DatabaseConnectionStringProvider(configuration).GetConnectionString();

        // SQL Server DATETIME2 carries no zone, so without this every timestamp read back is DateTimeKind.Unspecified.
        SqlMapper.AddTypeHandler(new UtcDateTimeHandler());
        // Maps DATE columns to DateOnly explicitly, so the round-trip does not depend on the driver's own DateOnly support.
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        services
            .AddScoped<IDatabaseConnection, DatabaseConnection>()
            .AddScoped<IDapperDataContext, DapperDataContext>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IDbMigrator, DbMigrator>();

        services.AddScoped<ILeagueRepository, LeagueRepository>();
        services.AddScoped<ISeasonRepository, SeasonRepository>();
        services.AddScoped<IDivisionRepository, DivisionRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IPlayerPositionRepository, PlayerPositionRepository>();
        services.AddScoped<IStandingRepository, StandingRepository>();
        services.AddScoped<ILeagueTeamManagerRepository, LeagueTeamManagerRepository>();
        services.AddScoped<IManagerRepository, ManagerRepository>();
        services.AddScoped<IStadiumRepository, StadiumRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IMatchPlayerStatisticRepository, MatchPlayerStatisticRepository>();
        services.AddScoped<IMatchTeamStatisticRepository, MatchTeamStatisticRepository>();
        services.AddScoped<ILeagueTeamPlayerRepository, LeagueTeamPlayerRepository>();
        services.AddScoped<IMatchTeamTacticRepository, MatchTeamTacticRepository>();
        services.AddScoped<IFormationRepository, FormationRepository>();

        services.AddHealthChecks()
            .AddSqlServer(connectionString, name: "sqlserver");

        return services;
    }
}
