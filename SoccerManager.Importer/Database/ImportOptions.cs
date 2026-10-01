namespace SoccerManager.Importer.Database;

/// <summary>
/// Configuration for how the importer writes the import model to the database.
/// </summary>
public sealed class ImportOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Import";

    /// <summary>The number of rows written concurrently within each table, and the number of per-player position reads read concurrently.</summary>
    public int MaxDegreeOfParallelism { get; set; } = 8;
}
