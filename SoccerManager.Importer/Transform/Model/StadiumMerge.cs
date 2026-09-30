namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// A group of two or more imported clubs' stadium names that normalized to the same de-duplication key and were
/// merged into a single <see cref="ImportStadium"/>.
/// </summary>
/// <param name="Key">The shared de-duplication key.</param>
/// <param name="Names">The distinct trimmed stadium names that merged into this key.</param>
public sealed record StadiumMerge(string Key, IReadOnlyList<string> Names);
