using CsvHelper;

namespace SoccerManager.Importer.Dataset;

/// <summary>
/// Accumulates the in-scope players as the players table streams: those whose last season matches the configured
/// current season and whose current club plays in one of the configured leagues.
/// </summary>
internal sealed class PlayersAccumulator
{
    private readonly HashSet<string> _leagues;
    private readonly int _currentSeason;
    private readonly List<SourcePlayer> _players = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayersAccumulator"/> class.
    /// </summary>
    /// <param name="leagues">The Transfermarkt domestic competition codes in scope.</param>
    /// <param name="currentSeason">The season a player must have last played in to be kept.</param>
    public PlayersAccumulator(HashSet<string> leagues, int currentSeason)
    {
        _leagues = leagues;
        _currentSeason = currentSeason;
    }

    /// <summary>The players kept so far.</summary>
    public IReadOnlyList<SourcePlayer> Players => _players;

    /// <summary>Folds one players row into this accumulator, keeping it only when it is in scope.</summary>
    /// <param name="row">The current CSV record.</param>
    public void AddRow(IReaderRow row)
    {
        var lastSeason = row.GetField<int?>("last_season");
        var competitionId = row.GetField("current_club_domestic_competition_id");

        if (lastSeason != _currentSeason || string.IsNullOrEmpty(competitionId) || !_leagues.Contains(competitionId))
        {
            return;
        }

        _players.Add(new SourcePlayer(
            row.GetField<int>("player_id"),
            row.GetField("name") ?? string.Empty,
            CsvFieldReader.NullIfBlank(row.GetField("date_of_birth")),
            CsvFieldReader.NullIfBlank(row.GetField("position")),
            CsvFieldReader.NullIfBlank(row.GetField("sub_position")),
            row.GetField<decimal?>("market_value_in_eur"),
            row.GetField<int?>("current_club_id"),
            row.GetField<int?>("current_national_team_id"),
            row.GetField<int?>("international_caps") ?? 0,
            CsvFieldReader.NullIfBlank(row.GetField("image_url"))));
    }
}
