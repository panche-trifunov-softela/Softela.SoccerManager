namespace SoccerManager.Importer.Dataset;

/// <summary>
/// One in-scope player's starting lineup appearances in window games, by normalised position name.
/// </summary>
/// <param name="PlayerTransfermarktId">The Transfermarkt player id.</param>
/// <param name="StartsByPosition">The number of starts at each normalised position name the player started in.</param>
public sealed record PlayerPositionStarts(int PlayerTransfermarktId, IReadOnlyDictionary<string, int> StartsByPosition);
