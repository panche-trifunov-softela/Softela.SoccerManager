using System.Collections.Concurrent;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// A thread-safe key-to-database-id map, written once per key by the planner or a row's own write, and read by
/// every row that links to that key, however many of those run in parallel.
/// </summary>
/// <typeparam name="TKey">The natural-key type: a stadium or position name, or a Transfermarkt id.</typeparam>
public sealed class IdLookup<TKey> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, int> _ids;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdLookup{TKey}"/> class.
    /// </summary>
    /// <param name="comparer">The equality comparer keys are compared with, or <see langword="null"/> for the default comparer.</param>
    public IdLookup(IEqualityComparer<TKey>? comparer = null)
    {
        _ids = comparer is null ? new ConcurrentDictionary<TKey, int>() : new ConcurrentDictionary<TKey, int>(comparer);
    }

    /// <summary>
    /// Records the database id for a key, overwriting any id already recorded for it.
    /// </summary>
    /// <param name="key">The natural key.</param>
    /// <param name="id">The database id the key resolves to.</param>
    public void Set(TKey key, int id) => _ids[key] = id;

    /// <summary>
    /// Looks up the database id recorded for a key.
    /// </summary>
    /// <param name="key">The natural key.</param>
    /// <returns>The recorded id, or <see langword="null"/> when the key has not been seeded or written yet.</returns>
    public int? Find(TKey key) => _ids.TryGetValue(key, out var id) ? id : null;
}
