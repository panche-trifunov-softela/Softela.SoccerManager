namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// A source row the transform step declined to carry into the import model.
/// </summary>
/// <param name="Entity">The kind of row, e.g. "Player", "Team" or "Stadium".</param>
/// <param name="Key">An identifying key for the row, such as its Transfermarkt id or raw name.</param>
/// <param name="Reason">A short, human-readable reason it was dropped.</param>
public sealed record DroppedRow(string Entity, string Key, string Reason);
