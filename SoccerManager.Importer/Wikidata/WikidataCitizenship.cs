namespace SoccerManager.Importer.Wikidata;

/// <summary>
/// A country of citizenship (wdt:P27) Wikidata records for a player.
/// </summary>
/// <param name="Qid">The Wikidata item id of the country, e.g. "Q142".</param>
/// <param name="Label">The country's English label.</param>
public sealed record WikidataCitizenship(string Qid, string Label);
