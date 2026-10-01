namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// The id lookups every table's writes resolve their links through: seeded by each table's planner with the ids of
/// the existing rows it matched, and filled in further as this run's own creates complete.
/// </summary>
public sealed class ImportIds
{
    /// <summary>Stadium database ids, keyed by <see cref="SoccerManager.Importer.Transform.NameNormalizer"/>'s normalized stadium name.</summary>
    public IdLookup<string> Stadiums { get; } = new(StringComparer.Ordinal);

    /// <summary>Position database ids, keyed by the position's exact name.</summary>
    public IdLookup<string> Positions { get; } = new(StringComparer.Ordinal);

    /// <summary>Team database ids, keyed by Transfermarkt id.</summary>
    public IdLookup<int> Teams { get; } = new();

    /// <summary>National team database ids, keyed by Transfermarkt id.</summary>
    public IdLookup<int> NationalTeams { get; } = new();

    /// <summary>Player database ids, keyed by Transfermarkt id.</summary>
    public IdLookup<int> Players { get; } = new();
}
