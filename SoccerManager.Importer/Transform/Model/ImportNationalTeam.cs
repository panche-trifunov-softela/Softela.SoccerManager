namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// A national team actually referenced by an imported player.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt national team id.</param>
/// <param name="Name">The team's name.</param>
/// <param name="LogoUrl">The team's logo image URL, or <see langword="null"/> when blank or longer than 500 characters.</param>
public sealed record ImportNationalTeam(int TransfermarktId, string Name, string? LogoUrl);
