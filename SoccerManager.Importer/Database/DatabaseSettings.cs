namespace SoccerManager.Importer.Database;

/// <summary>
/// Whether the importer has a usable "soccermanager" connection string, decided once at startup so every later
/// step only needs to check this flag instead of re-reading configuration.
/// </summary>
/// <param name="IsConfigured">Whether ConnectionStrings:soccermanager is set to a non-blank value.</param>
public sealed record DatabaseSettings(bool IsConfigured);
