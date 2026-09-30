namespace SoccerManager.Importer.Dataset;

/// <summary>
/// The 13 position names a game_lineups row's raw position is normalised against. Kept local to the dataset step so
/// a player's lineup starts can be counted before the transform step's own position catalog exists; that catalog
/// must keep the same 13 names, spelled exactly as here, since both describe the same fixed set of positions.
/// </summary>
internal static class LineupPositionNames
{
    /// <summary>The 13 recognised position names, exactly as the dataset spells them.</summary>
    public static IReadOnlyCollection<string> All { get; } = new[]
    {
        "Goalkeeper", "Centre-Back", "Left-Back", "Right-Back", "Defensive Midfield", "Central Midfield",
        "Attacking Midfield", "Left Midfield", "Right Midfield", "Left Winger", "Right Winger", "Second Striker",
        "Centre-Forward",
    };
}
