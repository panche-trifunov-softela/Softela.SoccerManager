namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// An imported player.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt player id.</param>
/// <param name="Name">The player's trimmed name.</param>
/// <param name="DateOfBirth">The player's resolved date of birth.</param>
/// <param name="Rating">The player's overall rating, from 1 to 100.</param>
/// <param name="Value">The player's value in EUR.</param>
/// <param name="Wage">The player's estimated weekly wage in EUR.</param>
/// <param name="ImageUrl">The player's image URL, or <see langword="null"/> when blank, a placeholder, or longer than 500 characters.</param>
/// <param name="TeamTransfermarktId">The Transfermarkt id of the player's current club, or <see langword="null"/> when it has none or that club was not imported.</param>
/// <param name="NationalTeamTransfermarktId">The Transfermarkt id of the player's current national team, or <see langword="null"/> when it has none or that reference could not be resolved.</param>
/// <param name="Group">The position group the player's rating was computed relative to.</param>
/// <param name="Positions">The player's positions: the main one at quality 100, followed by any secondary ones.</param>
public sealed record ImportPlayer(
    int TransfermarktId,
    string Name,
    DateOnly DateOfBirth,
    int Rating,
    decimal Value,
    decimal Wage,
    string? ImageUrl,
    int? TeamTransfermarktId,
    int? NationalTeamTransfermarktId,
    PositionGroup Group,
    IReadOnlyList<ImportPlayerPosition> Positions);
