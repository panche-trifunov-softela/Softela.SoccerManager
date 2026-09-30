namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// One position a player can play, with how well they play it there.
/// </summary>
/// <param name="PositionName">The position's name, exactly as spelled in <see cref="PositionCatalog"/>.</param>
/// <param name="Quality">The player's quality at this position, from 0 to 100. The main position is always 100.</param>
public sealed record ImportPlayerPosition(string PositionName, int Quality);
