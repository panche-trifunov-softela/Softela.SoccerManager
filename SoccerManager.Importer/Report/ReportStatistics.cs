namespace SoccerManager.Importer.Report;

/// <summary>
/// Percentile and histogram helpers used by the dry-run report's ratings and wages sections.
/// </summary>
public static class ReportStatistics
{
    /// <summary>
    /// Linearly interpolates the value at <paramref name="percentile"/> within <paramref name="sortedAscending"/>.
    /// </summary>
    /// <param name="sortedAscending">The cohort's values, sorted ascending. Must not be empty.</param>
    /// <param name="percentile">The percentile to read, from 0.0 to 1.0.</param>
    /// <returns>The interpolated value at that percentile.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="sortedAscending"/> is empty.</exception>
    public static double PercentileOf(IReadOnlyList<double> sortedAscending, double percentile)
    {
        ArgumentNullException.ThrowIfNull(sortedAscending);

        if (sortedAscending.Count == 0)
        {
            throw new ArgumentException("The cohort must not be empty.", nameof(sortedAscending));
        }

        if (sortedAscending.Count == 1)
        {
            return sortedAscending[0];
        }

        var position = percentile * (sortedAscending.Count - 1);
        var lowerIndex = (int)Math.Floor(position);
        var upperIndex = (int)Math.Ceiling(position);

        if (lowerIndex == upperIndex)
        {
            return sortedAscending[lowerIndex];
        }

        var fraction = position - lowerIndex;
        return sortedAscending[lowerIndex] + ((sortedAscending[upperIndex] - sortedAscending[lowerIndex]) * fraction);
    }

    /// <summary>
    /// Groups ratings into 5-point bands ("1-5", "6-10", ... "96-100"), counting how many ratings fall into each.
    /// </summary>
    /// <param name="ratings">The ratings to bucket, each expected to be from 1 to 100.</param>
    /// <returns>Each populated band's label and count, ordered by band ascending.</returns>
    public static IReadOnlyList<(string Band, int Count)> HistogramBands(IReadOnlyList<int> ratings)
    {
        ArgumentNullException.ThrowIfNull(ratings);

        var countsByBandStart = new SortedDictionary<int, int>();
        foreach (var rating in ratings)
        {
            var bandStart = (((rating - 1) / 5) * 5) + 1;
            countsByBandStart[bandStart] = countsByBandStart.GetValueOrDefault(bandStart) + 1;
        }

        return countsByBandStart
            .Select(entry => ($"{entry.Key}-{entry.Key + 4}", entry.Value))
            .ToArray();
    }
}
