using CsvHelper;

namespace SoccerManager.Importer.Dataset;

/// <summary>
/// Accumulates the in-scope clubs as the clubs table streams: those playing in one of the configured leagues whose
/// last season matches the configured current season.
/// </summary>
internal sealed class ClubsAccumulator
{
    private readonly HashSet<string> _leagues;
    private readonly int _currentSeason;
    private readonly List<SourceClub> _clubs = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ClubsAccumulator"/> class.
    /// </summary>
    /// <param name="leagues">The Transfermarkt domestic competition codes in scope.</param>
    /// <param name="currentSeason">The season a club must have last played in to be kept.</param>
    public ClubsAccumulator(HashSet<string> leagues, int currentSeason)
    {
        _leagues = leagues;
        _currentSeason = currentSeason;
    }

    /// <summary>The clubs kept so far.</summary>
    public IReadOnlyList<SourceClub> Clubs => _clubs;

    /// <summary>Folds one clubs row into this accumulator, keeping it only when it is in scope.</summary>
    /// <param name="row">The current CSV record.</param>
    public void AddRow(IReaderRow row)
    {
        var competitionId = row.GetField("domestic_competition_id");
        var lastSeason = row.GetField<int?>("last_season");

        if (string.IsNullOrEmpty(competitionId) || !_leagues.Contains(competitionId) || lastSeason != _currentSeason)
        {
            return;
        }

        _clubs.Add(new SourceClub(
            row.GetField<int>("club_id"),
            row.GetField("name") ?? string.Empty,
            competitionId,
            CsvFieldReader.NullIfBlank(row.GetField("stadium_name")),
            row.GetField<int?>("stadium_seats"),
            CsvFieldReader.NullIfBlank(row.GetField("net_transfer_record"))));
    }
}
