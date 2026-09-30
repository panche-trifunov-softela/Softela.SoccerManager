namespace SoccerManager.Importer.Dataset;

/// <summary>
/// A national team, as read from the national_teams table. Every row is kept: the table itself is small, and which
/// teams are actually referenced only becomes clear once the in-scope players are known.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt national team id.</param>
/// <param name="Name">The team's name, as recorded on Transfermarkt.</param>
/// <param name="ImageUrl">The team's logo image URL, or <see langword="null"/> when blank.</param>
public sealed record SourceNationalTeam(int TransfermarktId, string Name, string? ImageUrl);
