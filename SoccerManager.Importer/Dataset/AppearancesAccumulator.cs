using CsvHelper;

namespace SoccerManager.Importer.Dataset;

/// <summary>
/// Accumulates each in-scope player's per-season domestic league totals and whole-window UEFA totals as the
/// appearances table streams. A row is kept when its game is a window game and its player is in scope; which
/// totals it feeds depends on whether that window game is a league game or a UEFA game.
/// </summary>
internal sealed class AppearancesAccumulator
{
    private readonly IReadOnlyDictionary<int, WindowGame> _windowGames;
    private readonly HashSet<int> _inScopePlayerIds;
    private readonly Dictionary<(int Player, int Season), SeasonTotals> _seasonTotals = new();
    private readonly Dictionary<int, UefaTotals> _uefaTotals = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AppearancesAccumulator"/> class.
    /// </summary>
    /// <param name="windowGames">The window games kept while streaming the games table, by game id.</param>
    /// <param name="inScopePlayerIds">The Transfermarkt ids of the in-scope players.</param>
    public AppearancesAccumulator(IReadOnlyDictionary<int, WindowGame> windowGames, HashSet<int> inScopePlayerIds)
    {
        _windowGames = windowGames;
        _inScopePlayerIds = inScopePlayerIds;
    }

    /// <summary>The number of rows kept so far (game in the window and player in scope).</summary>
    public int RowsKept { get; private set; }

    /// <summary>Each in-scope player's domestic league appearance totals, per window season.</summary>
    public IReadOnlyList<PlayerSeasonStats> PlayerSeasonStats => _seasonTotals
        .Select(pair => new PlayerSeasonStats(
            pair.Key.Player,
            pair.Key.Season,
            pair.Value.Minutes,
            pair.Value.Goals,
            pair.Value.Assists,
            pair.Value.Appearances,
            pair.Value.MainClub(),
            pair.Value.MinutesByClub))
        .ToArray();

    /// <summary>Each in-scope player's UEFA competition totals, combined across the window.</summary>
    public IReadOnlyList<PlayerUefaStats> PlayerUefaStats => _uefaTotals
        .Select(pair => new PlayerUefaStats(pair.Key, pair.Value.Minutes, pair.Value.Goals, pair.Value.Assists))
        .ToArray();

    /// <summary>Folds one appearances row into this accumulator, keeping it only when it is in scope.</summary>
    /// <param name="row">The current CSV record.</param>
    public void AddRow(IReaderRow row)
    {
        var gameId = row.GetField<int>("game_id");
        var playerId = row.GetField<int>("player_id");

        if (!_windowGames.TryGetValue(gameId, out var game) || !_inScopePlayerIds.Contains(playerId))
        {
            return;
        }

        RowsKept++;

        var minutes = row.GetField<int?>("minutes_played") ?? 0;
        var goals = row.GetField<int?>("goals") ?? 0;
        var assists = row.GetField<int?>("assists") ?? 0;

        if (!game.IsLeague)
        {
            GetOrAdd(_uefaTotals, playerId, static () => new UefaTotals()).Add(minutes, goals, assists);
            return;
        }

        var seasonKey = (playerId, game.Season);
        var seasonTotals = GetOrAdd(_seasonTotals, seasonKey, static () => new SeasonTotals());
        seasonTotals.Add(minutes, goals, assists, row.GetField<int?>("player_club_id"));
    }

    private static TValue GetOrAdd<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> create)
        where TKey : notnull
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = create();
            dictionary[key] = value;
        }

        return value;
    }
}
