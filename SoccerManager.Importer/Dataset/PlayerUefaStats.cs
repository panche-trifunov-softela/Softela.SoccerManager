namespace SoccerManager.Importer.Dataset;

/// <summary>
/// One in-scope player's UEFA club competition totals, combined across the whole window.
/// </summary>
/// <param name="PlayerTransfermarktId">The Transfermarkt player id.</param>
/// <param name="Minutes">The UEFA minutes played.</param>
/// <param name="Goals">The UEFA goals scored.</param>
/// <param name="Assists">The UEFA assists recorded.</param>
public sealed record PlayerUefaStats(int PlayerTransfermarktId, int Minutes, int Goals, int Assists);
