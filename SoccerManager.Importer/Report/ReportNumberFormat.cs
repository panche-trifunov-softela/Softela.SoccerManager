using System.Globalization;

namespace SoccerManager.Importer.Report;

/// <summary>
/// Culture-invariant number and duration formatting shared by the dry-run report's sections, so the report's output
/// is identical regardless of the machine's locale.
/// </summary>
public static class ReportNumberFormat
{
    /// <summary>Formats a whole count with thousands separators, e.g. "12,345".</summary>
    /// <param name="value">The count to format.</param>
    /// <returns>The formatted count.</returns>
    public static string Count(long value) => value.ToString("N0", CultureInfo.InvariantCulture);

    /// <summary>Formats a byte count in megabytes with one decimal, e.g. "42.7 MB".</summary>
    /// <param name="bytes">The byte count to format.</param>
    /// <returns>The formatted size.</returns>
    public static string Megabytes(long bytes) => (bytes / 1_000_000.0).ToString("N1", CultureInfo.InvariantCulture) + " MB";

    /// <summary>Formats a euro amount in millions with one decimal, e.g. "12.3m".</summary>
    /// <param name="value">The amount in EUR to format.</param>
    /// <returns>The formatted amount.</returns>
    public static string EurMillions(decimal value) => (value / 1_000_000m).ToString("N1", CultureInfo.InvariantCulture) + "m";

    /// <summary>Formats a whole euro amount with thousands separators and no decimals, e.g. "1,234".</summary>
    /// <param name="value">The amount in EUR to format.</param>
    /// <returns>The formatted amount.</returns>
    public static string Eur(decimal value) => value.ToString("N0", CultureInfo.InvariantCulture);

    /// <summary>Formats a duration in whole seconds with one decimal, e.g. "12.3s".</summary>
    /// <param name="duration">The duration to format.</param>
    /// <returns>The formatted duration.</returns>
    public static string Seconds(TimeSpan duration) => duration.TotalSeconds.ToString("N1", CultureInfo.InvariantCulture) + "s";

    /// <summary>Formats a number with one decimal, e.g. "70.4".</summary>
    /// <param name="value">The number to format.</param>
    /// <returns>The formatted number.</returns>
    public static string OneDecimal(double value) => value.ToString("N1", CultureInfo.InvariantCulture);
}
