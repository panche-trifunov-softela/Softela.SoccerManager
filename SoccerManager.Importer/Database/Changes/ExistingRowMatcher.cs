namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Matches existing database rows to a name key for the three name-keyed tables (stadiums, positions, referees):
/// several existing rows can legitimately share a key (two clubs' grounds normalizing to the same stadium name, a
/// positions or referees table a developer seeded by hand), and only one of them can be the match a create would
/// otherwise duplicate.
/// </summary>
public static class ExistingRowMatcher
{
    /// <summary>
    /// Groups <paramref name="rows"/> by <paramref name="keySelector"/> and keeps the lowest-id row in each group,
    /// recording a note for every key more than one row shares.
    /// </summary>
    /// <typeparam name="TRow">The existing row's DTO type.</typeparam>
    /// <param name="rows">The existing rows to match.</param>
    /// <param name="keySelector">Builds a row's name key.</param>
    /// <param name="idSelector">Reads a row's database id.</param>
    /// <param name="notes">The note list a multi-row key is reported to.</param>
    /// <returns>The chosen row for each distinct key, using <see cref="StringComparer.Ordinal"/>.</returns>
    public static Dictionary<string, TRow> MatchByKey<TRow>(
        IReadOnlyList<TRow> rows,
        Func<TRow, string> keySelector,
        Func<TRow, int> idSelector,
        List<string> notes)
    {
        var result = new Dictionary<string, TRow>(StringComparer.Ordinal);

        foreach (var group in rows.GroupBy(keySelector, StringComparer.Ordinal))
        {
            var ordered = group.OrderBy(idSelector).ToList();
            var chosen = ordered[0];

            result[group.Key] = chosen;

            if (ordered.Count > 1)
            {
                var ids = string.Join(", ", ordered.Select(idSelector));
                notes.Add($"'{group.Key}' matches {ordered.Count} rows (ids {ids}); using id {idSelector(chosen)}.");
            }
        }

        return result;
    }
}
