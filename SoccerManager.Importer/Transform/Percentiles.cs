namespace SoccerManager.Importer.Transform;

/// <summary>
/// Computes rank percentiles for a cohort of values, used to turn a raw rating component into a value comparable
/// across players.
/// </summary>
public static class Percentiles
{
    /// <summary>
    /// Ranks each value against the others in <paramref name="values"/>: values are given ascending ranks 1..n, with
    /// tied values sharing the average of the ranks they span, and each rank is converted to a percentile in [0,1]
    /// via <c>p = (rank - 1) / (n - 1)</c>. A single-value cohort maps that value to 1.0.
    /// </summary>
    /// <param name="values">The cohort's values, in the order percentiles should be returned in.</param>
    /// <returns>Each input value's percentile, in the same order as <paramref name="values"/>.</returns>
    public static IReadOnlyList<double> Rank(IReadOnlyList<double> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        var count = values.Count;
        var result = new double[count];

        if (count == 0)
        {
            return result;
        }

        if (count == 1)
        {
            result[0] = 1.0;
            return result;
        }

        var order = new int[count];
        for (var i = 0; i < count; i++)
        {
            order[i] = i;
        }

        Array.Sort(order, (left, right) => values[left].CompareTo(values[right]));

        var index = 0;
        while (index < count)
        {
            var tieEnd = index;
            while (tieEnd + 1 < count && values[order[tieEnd + 1]] == values[order[index]])
            {
                tieEnd++;
            }

            var averageRank = ((index + 1) + (tieEnd + 1)) / 2.0;
            var percentile = (averageRank - 1) / (count - 1);

            for (var tieIndex = index; tieIndex <= tieEnd; tieIndex++)
            {
                result[order[tieIndex]] = percentile;
            }

            index = tieEnd + 1;
        }

        return result;
    }
}
