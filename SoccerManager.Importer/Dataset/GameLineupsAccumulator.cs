using CsvHelper;

namespace SoccerManager.Importer.Dataset;

/// <summary>
/// Accumulates each in-scope player's starting lineup appearances, by normalised position, as the game_lineups
/// table streams. A row is kept when its game is a window game, its player is in scope, and its type is
/// "starting_lineup". A kept row's raw position is normalised against <see cref="LineupPositionNames"/> (with
/// "Sweeper" mapped to "Centre-Back"); a raw value that does not normalise is counted as an anomaly instead of a
/// start.
/// </summary>
internal sealed class GameLineupsAccumulator
{
    private const string StartingLineupType = "starting_lineup";
    private const string SweeperRawPosition = "Sweeper";
    private const string SweeperNormalizedPosition = "Centre-Back";

    private readonly IReadOnlyDictionary<int, WindowGame> _windowGames;
    private readonly HashSet<int> _inScopePlayerIds;
    private readonly HashSet<string> _validPositions;
    private readonly Dictionary<int, Dictionary<string, int>> _startsByPlayer = new();
    private readonly Dictionary<string, int> _anomalies = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="GameLineupsAccumulator"/> class.
    /// </summary>
    /// <param name="windowGames">The window games kept while streaming the games table, by game id.</param>
    /// <param name="inScopePlayerIds">The Transfermarkt ids of the in-scope players.</param>
    public GameLineupsAccumulator(IReadOnlyDictionary<int, WindowGame> windowGames, HashSet<int> inScopePlayerIds)
    {
        _windowGames = windowGames;
        _inScopePlayerIds = inScopePlayerIds;
        _validPositions = new HashSet<string>(LineupPositionNames.All, StringComparer.Ordinal);
    }

    /// <summary>The number of rows kept so far (window game, in-scope player, starting lineup).</summary>
    public int RowsKept { get; private set; }

    /// <summary>Each in-scope player's starts per normalised position.</summary>
    public IReadOnlyList<PlayerPositionStarts> PlayerPositionStarts => _startsByPlayer
        .Select(pair => new PlayerPositionStarts(pair.Key, pair.Value))
        .ToArray();

    /// <summary>The raw lineup position values that could not be normalised, with their occurrence counts.</summary>
    public IReadOnlyDictionary<string, int> Anomalies => _anomalies;

    /// <summary>Folds one game_lineups row into this accumulator, keeping it only when it is in scope.</summary>
    /// <param name="row">The current CSV record.</param>
    public void AddRow(IReaderRow row)
    {
        var gameId = row.GetField<int>("game_id");
        var playerId = row.GetField<int>("player_id");
        var type = row.GetField("type");

        if (!_windowGames.ContainsKey(gameId) || !_inScopePlayerIds.Contains(playerId)
            || !string.Equals(type, StartingLineupType, StringComparison.Ordinal))
        {
            return;
        }

        RowsKept++;

        var rawPosition = row.GetField("position") ?? string.Empty;
        var normalizedPosition = Normalize(rawPosition);

        if (normalizedPosition is null)
        {
            _anomalies.TryGetValue(rawPosition, out var anomalyCount);
            _anomalies[rawPosition] = anomalyCount + 1;
            return;
        }

        if (!_startsByPlayer.TryGetValue(playerId, out var starts))
        {
            starts = new Dictionary<string, int>(StringComparer.Ordinal);
            _startsByPlayer[playerId] = starts;
        }

        starts.TryGetValue(normalizedPosition, out var startCount);
        starts[normalizedPosition] = startCount + 1;
    }

    private string? Normalize(string rawPosition)
    {
        if (string.Equals(rawPosition, SweeperRawPosition, StringComparison.Ordinal))
        {
            return SweeperNormalizedPosition;
        }

        return _validPositions.Contains(rawPosition) ? rawPosition : null;
    }
}
