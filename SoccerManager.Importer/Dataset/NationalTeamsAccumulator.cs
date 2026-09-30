using CsvHelper;

namespace SoccerManager.Importer.Dataset;

/// <summary>
/// Accumulates every national team as the national_teams table streams. The table is small enough, and which teams
/// are actually referenced only becomes clear once the in-scope players are known, so every row is kept.
/// </summary>
internal sealed class NationalTeamsAccumulator
{
    private readonly List<SourceNationalTeam> _nationalTeams = new();

    /// <summary>The national teams kept so far.</summary>
    public IReadOnlyList<SourceNationalTeam> NationalTeams => _nationalTeams;

    /// <summary>Folds one national_teams row into this accumulator.</summary>
    /// <param name="row">The current CSV record.</param>
    public void AddRow(IReaderRow row)
    {
        _nationalTeams.Add(new SourceNationalTeam(
            row.GetField<int>("national_team_id"),
            row.GetField("name") ?? string.Empty,
            CsvFieldReader.NullIfBlank(row.GetField("team_image_url"))));
    }
}
