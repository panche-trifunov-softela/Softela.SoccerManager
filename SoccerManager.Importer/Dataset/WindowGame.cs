namespace SoccerManager.Importer.Dataset;

/// <summary>
/// A window game's identifying details, kept after the games table streams so the appearances and game_lineups
/// tables can filter and aggregate by game id without needing to re-read the games table themselves.
/// </summary>
/// <param name="CompetitionId">The Transfermarkt competition code, e.g. "GB1" or "CL".</param>
/// <param name="Season">The season the game was played in, encoded by its start year.</param>
/// <param name="IsLeague">Whether the game belongs to one of the domestic leagues in scope, as opposed to a UEFA competition.</param>
internal sealed record WindowGame(string CompetitionId, int Season, bool IsLeague);
