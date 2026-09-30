namespace SoccerManager.Importer.Dataset;

/// <summary>
/// One club's domestic league results in a single window season.
/// </summary>
/// <param name="ClubTransfermarktId">The Transfermarkt club id.</param>
/// <param name="Season">The season these results were recorded in, encoded by its start year.</param>
/// <param name="Games">The number of league games played.</param>
/// <param name="Points">The league points earned (3 for a win, 1 for a draw, 0 for a loss).</param>
/// <param name="GoalsFor">The goals scored across those games.</param>
/// <param name="GoalsAgainst">The goals conceded across those games.</param>
public sealed record ClubSeasonLeagueStats(int ClubTransfermarktId, int Season, int Games, int Points, int GoalsFor, int GoalsAgainst);
