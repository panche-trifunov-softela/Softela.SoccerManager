namespace SoccerManager.Importer.Wikidata;

/// <summary>
/// A single date of birth statement Wikidata records for a player, together with its precision.
/// </summary>
/// <param name="Date">The date, as yyyy-MM-dd.</param>
/// <param name="Precision">The Wikidata time precision (11 = day, 10 = month, 9 = year, and so on).</param>
public sealed record WikidataDateOfBirth(string Date, int Precision);
