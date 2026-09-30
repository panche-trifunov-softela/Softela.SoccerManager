namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// A stadium built from the imported clubs' stadium names, de-duplicated by their normalized name.
/// </summary>
/// <param name="Key">The stadium's de-duplication key, from <see cref="NameNormalizer"/>.</param>
/// <param name="Name">The first-seen trimmed stadium name for this key.</param>
/// <param name="Size">The largest seat count recorded among the clubs sharing this key.</param>
public sealed record ImportStadium(string Key, string Name, int Size);
