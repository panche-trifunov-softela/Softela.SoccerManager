namespace SoccerManager.Importer.Database.Writing;

/// <summary>
/// One row that was not written, because its command failed or because it was skipped.
/// </summary>
/// <param name="Entity">The table's display name.</param>
/// <param name="Key">The row's natural-key display key, as planned by its <see cref="Changes.RowWrite"/>.</param>
/// <param name="Name">The row's display name.</param>
/// <param name="Reason">Why the row was not written.</param>
public sealed record WriteFailure(string Entity, string Key, string Name, string Reason);
