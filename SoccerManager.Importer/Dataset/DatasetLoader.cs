using CsvHelper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SoccerManager.Importer.Scope;
using SoccerManager.Importer.Source;

namespace SoccerManager.Importer.Dataset;

/// <summary>
/// Streams the dataset snapshot's seven tables, in load order, and folds each into the in-memory <see cref="Dataset"/>
/// later steps build the import model from.
/// </summary>
public sealed class DatasetLoader
{
    private readonly DatasetStreamReader _reader;
    private readonly ScopeOptions _scope;
    private readonly ILogger<DatasetLoader> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatasetLoader"/> class.
    /// </summary>
    /// <param name="reader">Streams a single table with retry.</param>
    /// <param name="scope">The league, UEFA competition and season scope configuration.</param>
    /// <param name="logger">The logger used to report per-table provenance.</param>
    public DatasetLoader(DatasetStreamReader reader, IOptions<ScopeOptions> scope, ILogger<DatasetLoader> logger)
    {
        _reader = reader;
        _scope = scope.Value;
        _logger = logger;
    }

    /// <summary>
    /// Streams and folds every table in load order: players, clubs, national_teams, games, appearances,
    /// game_lineups, player_valuations. Later tables filter by the in-scope players and window games established
    /// by the earlier ones, so the order matters.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the load.</param>
    /// <returns>The folded <see cref="Dataset"/>.</returns>
    public async Task<Dataset> LoadAsync(CancellationToken cancellationToken)
    {
        var leagues = new HashSet<string>(_scope.Leagues, StringComparer.Ordinal);
        var uefaCompetitions = new HashSet<string>(_scope.UefaCompetitions, StringComparer.Ordinal);
        var windowSeasons = new HashSet<int> { _scope.CurrentSeason - 1, _scope.CurrentSeason };
        var tableResults = new List<TableKeptResult>();

        var players = await ReadTableAsync(
            "players",
            () => new PlayersAccumulator(leagues, _scope.CurrentSeason),
            static (accumulator, row) => accumulator.AddRow(row),
            static accumulator => accumulator.Players.Count,
            tableResults,
            cancellationToken).ConfigureAwait(false);

        var inScopePlayerIds = new HashSet<int>(players.Players.Select(player => player.TransfermarktId));

        var clubs = await ReadTableAsync(
            "clubs",
            () => new ClubsAccumulator(leagues, _scope.CurrentSeason),
            static (accumulator, row) => accumulator.AddRow(row),
            static accumulator => accumulator.Clubs.Count,
            tableResults,
            cancellationToken).ConfigureAwait(false);

        var nationalTeams = await ReadTableAsync(
            "national_teams",
            static () => new NationalTeamsAccumulator(),
            static (accumulator, row) => accumulator.AddRow(row),
            static accumulator => accumulator.NationalTeams.Count,
            tableResults,
            cancellationToken).ConfigureAwait(false);

        var games = await ReadTableAsync(
            "games",
            () => new GamesAccumulator(leagues, uefaCompetitions, windowSeasons),
            static (accumulator, row) => accumulator.AddRow(row),
            static accumulator => accumulator.WindowGames.Count,
            tableResults,
            cancellationToken).ConfigureAwait(false);

        var appearances = await ReadTableAsync(
            "appearances",
            () => new AppearancesAccumulator(games.WindowGames, inScopePlayerIds),
            static (accumulator, row) => accumulator.AddRow(row),
            static accumulator => accumulator.RowsKept,
            tableResults,
            cancellationToken).ConfigureAwait(false);

        var gameLineups = await ReadTableAsync(
            "game_lineups",
            () => new GameLineupsAccumulator(games.WindowGames, inScopePlayerIds),
            static (accumulator, row) => accumulator.AddRow(row),
            static accumulator => accumulator.RowsKept,
            tableResults,
            cancellationToken).ConfigureAwait(false);

        var playerValuations = await ReadTableAsync(
            "player_valuations",
            () => new PlayerValuationsAccumulator(inScopePlayerIds),
            static (accumulator, row) => accumulator.AddRow(row),
            static accumulator => accumulator.RowsKept,
            tableResults,
            cancellationToken).ConfigureAwait(false);

        return new Dataset(
            tableResults,
            players.Players,
            clubs.Clubs,
            nationalTeams.NationalTeams,
            games.ClubSeasonLeagueStats,
            games.ClubLeagueStats,
            games.RefereeNames,
            appearances.PlayerSeasonStats,
            appearances.PlayerUefaStats,
            gameLineups.PlayerPositionStarts,
            gameLineups.Anomalies,
            playerValuations.Valuations);
    }

    private async Task<T> ReadTableAsync<T>(
        string table,
        Func<T> createAccumulator,
        Action<T, IReaderRow> addRow,
        Func<T, int> countRowsKept,
        List<TableKeptResult> tableResults,
        CancellationToken cancellationToken)
    {
        var (accumulator, result) = await _reader.ReadAsync(table, createAccumulator, addRow, cancellationToken).ConfigureAwait(false);
        var rowsKept = countRowsKept(accumulator);

        tableResults.Add(new TableKeptResult(result, rowsKept));

        _logger.LogInformation(
            "Read {Table}: {Bytes} bytes, {RowsRead} rows read, {RowsKept} rows kept, sha256 {Sha256}, {Duration}.",
            table, result.Bytes, result.RowsRead, rowsKept, result.Sha256, result.Duration);

        return accumulator;
    }
}
