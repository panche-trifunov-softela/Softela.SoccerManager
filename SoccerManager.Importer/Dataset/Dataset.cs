namespace SoccerManager.Importer.Dataset;

/// <summary>
/// The full, in-memory result of streaming the dataset snapshot's tables and keeping only the rows in scope. This
/// is what <c>Transform.ImportModelBuilder</c> builds the import model from.
/// </summary>
/// <param name="TableResults">Per-table provenance: bytes read, rows read, rows kept, hash and duration, one per streamed table in load order.</param>
/// <param name="Players">The in-scope players.</param>
/// <param name="Clubs">The in-scope clubs.</param>
/// <param name="NationalTeams">Every national team in the table.</param>
/// <param name="ClubSeasonLeagueStats">Each club's domestic league results, aggregated per window season.</param>
/// <param name="ClubLeagueStats">Each club's domestic league results, aggregated across the whole window.</param>
/// <param name="RefereeNames">The raw, non-blank referee names recorded on domestic league window games.</param>
/// <param name="PlayerSeasonStats">Each in-scope player's domestic league appearance totals, per window season.</param>
/// <param name="PlayerUefaStats">Each in-scope player's UEFA competition totals, combined across the window.</param>
/// <param name="PlayerPositionStarts">Each in-scope player's starting lineup appearances, by normalised position.</param>
/// <param name="LineupPositionAnomalies">Raw lineup position values that could not be normalised, with their occurrence counts.</param>
/// <param name="PlayerValuations">Each in-scope player's latest recorded market valuation.</param>
public sealed record Dataset(
    IReadOnlyList<TableKeptResult> TableResults,
    IReadOnlyList<SourcePlayer> Players,
    IReadOnlyList<SourceClub> Clubs,
    IReadOnlyList<SourceNationalTeam> NationalTeams,
    IReadOnlyList<ClubSeasonLeagueStats> ClubSeasonLeagueStats,
    IReadOnlyList<ClubLeagueStats> ClubLeagueStats,
    IReadOnlyList<string> RefereeNames,
    IReadOnlyList<PlayerSeasonStats> PlayerSeasonStats,
    IReadOnlyList<PlayerUefaStats> PlayerUefaStats,
    IReadOnlyList<PlayerPositionStarts> PlayerPositionStarts,
    IReadOnlyDictionary<string, int> LineupPositionAnomalies,
    IReadOnlyList<PlayerValuation> PlayerValuations);
