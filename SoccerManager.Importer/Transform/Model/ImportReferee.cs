namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// A distinct referee name recorded on a domestic league window game.
/// </summary>
/// <param name="Name">The referee's trimmed name, with inner whitespace collapsed.</param>
public sealed record ImportReferee(string Name);
