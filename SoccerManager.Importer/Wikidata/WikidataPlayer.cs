namespace SoccerManager.Importer.Wikidata;

/// <summary>
/// A player matched on Wikidata by Transfermarkt player ID (P2446).
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt player id the match was looked up by.</param>
/// <param name="Qid">The matched Wikidata item id, e.g. "Q615".</param>
/// <param name="Label">The item's English label.</param>
/// <param name="DatesOfBirth">The distinct dates of birth Wikidata records for this item.</param>
/// <param name="Citizenships">The distinct countries of citizenship Wikidata records for this item.</param>
public sealed record WikidataPlayer(
    int TransfermarktId,
    string Qid,
    string Label,
    IReadOnlyCollection<WikidataDateOfBirth> DatesOfBirth,
    IReadOnlyCollection<WikidataCitizenship> Citizenships);
