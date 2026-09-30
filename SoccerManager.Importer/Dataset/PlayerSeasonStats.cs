namespace SoccerManager.Importer.Dataset;

/// <summary>
/// One in-scope player's domestic league appearance totals in a single window season.
/// </summary>
/// <param name="PlayerTransfermarktId">The Transfermarkt player id.</param>
/// <param name="Season">The season these totals were recorded in, encoded by its start year.</param>
/// <param name="LeagueMinutes">The league minutes played.</param>
/// <param name="LeagueGoals">The league goals scored.</param>
/// <param name="LeagueAssists">The league assists recorded.</param>
/// <param name="LeagueAppearances">The number of league games appeared in.</param>
/// <param name="MainClubTransfermarktId">
/// The Transfermarkt id of the club the player played the most league minutes for in this season, or
/// <see langword="null"/> when the player recorded no league minutes for any club this season.
/// </param>
/// <param name="LeagueMinutesByClub">The player's league minutes this season, keyed by the club's Transfermarkt id.</param>
public sealed record PlayerSeasonStats(
    int PlayerTransfermarktId,
    int Season,
    int LeagueMinutes,
    int LeagueGoals,
    int LeagueAssists,
    int LeagueAppearances,
    int? MainClubTransfermarktId,
    IReadOnlyDictionary<int, int> LeagueMinutesByClub);
