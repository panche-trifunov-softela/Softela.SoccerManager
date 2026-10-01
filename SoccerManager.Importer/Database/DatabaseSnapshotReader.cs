using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Queries.NationalTeam.GetNationalTeams;
using SoccerManager.Application.Queries.Player.GetPlayers;
using SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositions;
using SoccerManager.Application.Queries.Position.GetPositions;
using SoccerManager.Application.Queries.Referee.GetReferees;
using SoccerManager.Application.Queries.Stadium.GetStadiums;
using SoccerManager.Application.Queries.Team.GetTeams;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Database;

/// <summary>
/// Checks the database connection and reads the snapshot the import plan is built against.
/// </summary>
public sealed class DatabaseSnapshotReader
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ImportOptions _options;
    private readonly ILogger<DatabaseSnapshotReader> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseSnapshotReader"/> class.
    /// </summary>
    /// <param name="scopeFactory">Creates the per-call dependency injection scopes the application services resolve from.</param>
    /// <param name="options">The import run's configuration, including its write parallelism.</param>
    /// <param name="logger">The logger used to report the snapshot's size and the connection check's outcome.</param>
    public DatabaseSnapshotReader(IServiceScopeFactory scopeFactory, IOptions<ImportOptions> options, ILogger<DatabaseSnapshotReader> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Runs one small query against the database, so a bad connection string or an unreachable server fails in
    /// seconds, before the dataset download and the Wikidata step have spent any time at all.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the check.</param>
    public async Task CheckConnectionAsync(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();

        await queryDispatcher.QueryAsync(new GetPositionsRequest(), cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Database connection check succeeded.");
    }

    /// <summary>
    /// Reads every table the import plan compares against: the six entity tables in one scope, then the position
    /// ratings of each database player the import model references, one scope per player, in parallel.
    /// </summary>
    /// <param name="model">The import model, whose players decide which database players' position ratings are read.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the read.</param>
    /// <returns>The database snapshot the import plan is built from.</returns>
    public async Task<DatabaseSnapshot> ReadAsync(ImportModel model, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        IReadOnlyList<StadiumDto> stadiums;
        IReadOnlyList<PositionDto> positions;
        IReadOnlyList<RefereeDto> referees;
        IReadOnlyList<TeamDto> teams;
        IReadOnlyList<NationalTeamDto> nationalTeams;
        IReadOnlyList<PlayerDto> players;

        await using (var scope = _scopeFactory.CreateAsyncScope())
        {
            var queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();

            stadiums = (await queryDispatcher.QueryAsync(new GetStadiumsRequest(), cancellationToken).ConfigureAwait(false)).Data;
            positions = (await queryDispatcher.QueryAsync(new GetPositionsRequest(), cancellationToken).ConfigureAwait(false)).Data;
            referees = (await queryDispatcher.QueryAsync(new GetRefereesRequest(), cancellationToken).ConfigureAwait(false)).Data;
            teams = (await queryDispatcher.QueryAsync(new GetTeamsRequest(), cancellationToken).ConfigureAwait(false)).Data;
            nationalTeams = (await queryDispatcher.QueryAsync(new GetNationalTeamsRequest(), cancellationToken).ConfigureAwait(false)).Data;
            players = (await queryDispatcher.QueryAsync(new GetPlayersRequest(), cancellationToken).ConfigureAwait(false)).Data;
        }

        // Only players the import model actually references need their position ratings read: there is no GetAll
        // query for player positions, so every other database player's ratings would otherwise cost a wasted read.
        var transfermarktIdsInModel = new HashSet<int>(model.Players.Select(player => player.TransfermarktId));
        var playersInScope = players.Where(player => player.TransfermarktId is { } id && transfermarktIdsInModel.Contains(id)).ToArray();

        var playerPositionsByPlayerId = new ConcurrentDictionary<int, IReadOnlyList<PlayerPositionDto>>();

        await Parallel.ForEachAsync(
            playersInScope,
            new ParallelOptions { MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism, CancellationToken = cancellationToken },
            async (player, token) =>
            {
                // One scope per player, like every other database access here: each scope gets its own
                // UnitOfWork and connection, so the parallel reads do not share state.
                await using var scope = _scopeFactory.CreateAsyncScope();
                var queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();

                var response = await queryDispatcher.QueryAsync(new GetPlayerPositionsRequest { PlayerId = player.Id }, token).ConfigureAwait(false);

                playerPositionsByPlayerId[player.Id] = response.Data;
            }).ConfigureAwait(false);

        stopwatch.Stop();

        _logger.LogInformation(
            "Database snapshot: {Stadiums} stadiums, {Positions} positions, {Referees} referees, {Teams} teams, {NationalTeams} national teams, {Players} players, {PlayerPositionReads} player position reads in {Elapsed}.",
            stadiums.Count, positions.Count, referees.Count, teams.Count, nationalTeams.Count, players.Count, playersInScope.Length, stopwatch.Elapsed);

        return new DatabaseSnapshot(stadiums, positions, referees, teams, nationalTeams, players, playerPositionsByPlayerId);
    }
}
