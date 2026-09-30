namespace SoccerManager.Importer.Report;

/// <summary>
/// Builds fixed-width plain-text table rows for the dry-run report's tabular sections.
/// </summary>
public static class ReportTable
{
    /// <summary>Right-aligns <paramref name="value"/> within <paramref name="width"/> characters, truncating if it is longer.</summary>
    /// <param name="value">The cell text.</param>
    /// <param name="width">The cell width.</param>
    /// <returns>The right-aligned, fixed-width cell.</returns>
    public static string Right(string value, int width) => Fit(value, width).PadLeft(width);

    /// <summary>Left-aligns <paramref name="value"/> within <paramref name="width"/> characters, truncating if it is longer.</summary>
    /// <param name="value">The cell text.</param>
    /// <param name="width">The cell width.</param>
    /// <returns>The left-aligned, fixed-width cell.</returns>
    public static string Left(string value, int width) => Fit(value, width).PadRight(width);

    /// <summary>Joins already fixed-width cells into one row, separated by two spaces.</summary>
    /// <param name="cells">The row's cells, each already padded to its column width.</param>
    /// <returns>The joined row.</returns>
    public static string Row(params string[] cells) => string.Join("  ", cells);

    // A cell wider than its column would push every following column out of alignment, so it is truncated rather
    // than left to overflow.
    private static string Fit(string value, int width) => value.Length > width ? value[..width] : value;
}
