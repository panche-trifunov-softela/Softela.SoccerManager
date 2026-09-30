using SoccerManager.Importer.Dataset;

namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// Everything the dry-run report needs beyond the import model's own entity lists: per-table provenance, the scope
/// actually applied, Wikidata enrichment counters, dropped rows, and the bookkeeping behind the entity
/// de-duplication and cleanup rules.
/// </summary>
/// <param name="TableResults">Per-table provenance, carried over from the streamed <see cref="Dataset.Dataset"/>.</param>
/// <param name="Leagues">The domestic league codes the run was scoped to.</param>
/// <param name="WindowSeasons">The window seasons the run was scoped to, encoded by their start year.</param>
/// <param name="WikidataSkipped">Whether the Wikidata enrichment step was skipped for this run.</param>
/// <param name="WikidataRequested">The number of in-scope players Wikidata was queried for.</param>
/// <param name="WikidataMatched">The number of those players Wikidata returned a match for.</param>
/// <param name="DatesOfBirthFilledFromWikidata">The number of players whose date of birth was filled from Wikidata because Transfermarkt's was missing or invalid.</param>
/// <param name="DobMismatches">Players whose Transfermarkt date of birth did not match any of Wikidata's day-precision dates.</param>
/// <param name="CitizenshipMismatches">Players whose Wikidata citizenships did not include their national team's country.</param>
/// <param name="DroppedRows">Every source row declined from the import model, with its reason.</param>
/// <param name="LineupPositionAnomalies">Raw lineup position values that did not normalize to one of the 13 positions, with their occurrence counts.</param>
/// <param name="StadiumMerges">Groups of clubs' stadium names that merged into a single stadium.</param>
/// <param name="PlaceholderImageUrlsRemoved">The distinct player image URLs removed for being shared by too many players.</param>
/// <param name="PlayersWithUnimportedTeam">The number of players whose current club is known but was not imported, so their team reference was nulled.</param>
/// <param name="PlayersWithUnresolvedNationalTeam">The number of players whose current national team id did not resolve in the national teams table, so their national team reference was nulled.</param>
public sealed record ImportDiagnostics(
    IReadOnlyList<TableKeptResult> TableResults,
    IReadOnlyList<string> Leagues,
    IReadOnlyList<int> WindowSeasons,
    bool WikidataSkipped,
    int WikidataRequested,
    int WikidataMatched,
    int DatesOfBirthFilledFromWikidata,
    IReadOnlyList<DobMismatch> DobMismatches,
    IReadOnlyList<CitizenshipMismatch> CitizenshipMismatches,
    IReadOnlyList<DroppedRow> DroppedRows,
    IReadOnlyDictionary<string, int> LineupPositionAnomalies,
    IReadOnlyList<StadiumMerge> StadiumMerges,
    IReadOnlyList<string> PlaceholderImageUrlsRemoved,
    int PlayersWithUnimportedTeam,
    int PlayersWithUnresolvedNationalTeam);
