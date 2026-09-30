namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// A player whose Transfermarkt date of birth does not match any of Wikidata's day-precision dates of birth. The
/// Transfermarkt date is still the one used; this is reported, not corrected.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt player id.</param>
/// <param name="Name">The player's name.</param>
/// <param name="TransfermarktDate">The Transfermarkt date of birth, as yyyy-MM-dd.</param>
/// <param name="WikidataDates">Wikidata's day-precision dates of birth, as yyyy-MM-dd.</param>
public sealed record DobMismatch(int TransfermarktId, string Name, string TransfermarktDate, IReadOnlyList<string> WikidataDates);
