namespace SoccerManager.Importer.Dataset;

/// <summary>
/// A small helper for reading optional CSV fields consistently across the dataset accumulators.
/// </summary>
internal static class CsvFieldReader
{
    /// <summary>
    /// Returns the given value, or <see langword="null"/> when it is empty or made up only of whitespace.
    /// </summary>
    /// <param name="value">The raw field value.</param>
    /// <returns>The value unchanged, or <see langword="null"/> when blank.</returns>
    public static string? NullIfBlank(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
