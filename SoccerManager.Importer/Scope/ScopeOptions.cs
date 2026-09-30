namespace SoccerManager.Importer.Scope;

/// <summary>
/// Configuration for the set of leagues, UEFA competitions and season the importer targets.
/// </summary>
public sealed class ScopeOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Scope";

    /// <summary>The Transfermarkt domestic competition codes the importer targets.</summary>
    public string[] Leagues { get; set; } = new[] { "GB1", "FR1", "NL1", "IT1", "L1", "ES1", "PO1" };

    /// <summary>The season a player must have last played in to be considered in scope.</summary>
    public int CurrentSeason { get; set; } = 2025;

    /// <summary>
    /// The Transfermarkt UEFA club competition codes included in the games window alongside <see cref="Leagues"/>,
    /// so a player's continental minutes and goals count even though the competition itself is not a domestic
    /// league.
    /// </summary>
    public string[] UefaCompetitions { get; set; } = new[] { "CL", "EL", "UCOL" };
}
