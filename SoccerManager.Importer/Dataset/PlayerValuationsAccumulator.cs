using CsvHelper;

namespace SoccerManager.Importer.Dataset;

/// <summary>
/// Accumulates each in-scope player's latest recorded market valuation as the player_valuations table streams. A
/// row is kept when its player is in scope; only the row with the latest date is retained per player.
/// </summary>
internal sealed class PlayerValuationsAccumulator
{
    private readonly HashSet<int> _inScopePlayerIds;
    private readonly Dictionary<int, (DateOnly Date, decimal Value)> _latestByPlayer = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerValuationsAccumulator"/> class.
    /// </summary>
    /// <param name="inScopePlayerIds">The Transfermarkt ids of the in-scope players.</param>
    public PlayerValuationsAccumulator(HashSet<int> inScopePlayerIds)
    {
        _inScopePlayerIds = inScopePlayerIds;
    }

    /// <summary>The number of rows kept so far (player in scope).</summary>
    public int RowsKept { get; private set; }

    /// <summary>Each in-scope player's latest recorded market valuation.</summary>
    public IReadOnlyList<PlayerValuation> Valuations => _latestByPlayer
        .Select(pair => new PlayerValuation(pair.Key, pair.Value.Value))
        .ToArray();

    /// <summary>Folds one player_valuations row into this accumulator, keeping it only when it is in scope.</summary>
    /// <param name="row">The current CSV record.</param>
    public void AddRow(IReaderRow row)
    {
        var playerId = row.GetField<int>("player_id");

        if (!_inScopePlayerIds.Contains(playerId))
        {
            return;
        }

        RowsKept++;

        var date = row.GetField<DateOnly?>("date");
        var value = row.GetField<decimal?>("market_value_in_eur");

        if (date is null || value is null)
        {
            return;
        }

        if (!_latestByPlayer.TryGetValue(playerId, out var current) || date.Value > current.Date)
        {
            _latestByPlayer[playerId] = (date.Value, value.Value);
        }
    }
}
