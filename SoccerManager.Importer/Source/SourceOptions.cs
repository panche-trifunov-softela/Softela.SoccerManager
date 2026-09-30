namespace SoccerManager.Importer.Source;

/// <summary>
/// Configuration for streaming the frozen dataset snapshot's tables.
/// </summary>
public sealed class SourceOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Source";

    /// <summary>The base URL each dataset table is streamed from, one file per table name.</summary>
    public string BaseUrl { get; set; } = "https://pub-e682421888d945d684bcae8890b0ec20.r2.dev/data/";

    /// <summary>The time allowed for a single table to finish streaming, before it is treated as a transient failure and retried.</summary>
    public int TableTimeoutMinutes { get; set; } = 15;

    /// <summary>The maximum number of attempts for a table read before it is considered failed.</summary>
    public int MaxAttempts { get; set; } = 3;
}
