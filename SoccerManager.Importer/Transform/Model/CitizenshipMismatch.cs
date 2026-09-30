namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// A player whose Wikidata citizenships do not include their Transfermarkt national team's country. Reported only;
/// the player's national team is not changed.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt player id.</param>
/// <param name="Name">The player's name.</param>
/// <param name="NationalTeamName">The player's Transfermarkt national team name.</param>
/// <param name="WikidataCitizenships">The countries of citizenship Wikidata records for the player.</param>
public sealed record CitizenshipMismatch(int TransfermarktId, string Name, string NationalTeamName, IReadOnlyList<string> WikidataCitizenships);
