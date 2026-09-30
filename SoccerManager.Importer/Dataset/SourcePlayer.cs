namespace SoccerManager.Importer.Dataset;

/// <summary>
/// An in-scope player, as read from the players table.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt player id.</param>
/// <param name="Name">The player's name, as recorded on Transfermarkt.</param>
/// <param name="DateOfBirthText">The raw date of birth text, or <see langword="null"/> when blank.</param>
/// <param name="Position">The player's main position name, or <see langword="null"/> when blank.</param>
/// <param name="SubPosition">The player's sub-position name, or <see langword="null"/> when blank.</param>
/// <param name="MarketValue">The player's recorded market value in EUR, or <see langword="null"/> when blank.</param>
/// <param name="CurrentClubId">The Transfermarkt id of the player's current club, or <see langword="null"/> when blank.</param>
/// <param name="CurrentNationalTeamId">The Transfermarkt id of the player's current national team, or <see langword="null"/> when blank.</param>
/// <param name="InternationalCaps">The player's international caps (0 when the source field was blank).</param>
/// <param name="ImageUrl">The player's image URL, or <see langword="null"/> when blank.</param>
public sealed record SourcePlayer(
    int TransfermarktId,
    string Name,
    string? DateOfBirthText,
    string? Position,
    string? SubPosition,
    decimal? MarketValue,
    int? CurrentClubId,
    int? CurrentNationalTeamId,
    int InternationalCaps,
    string? ImageUrl);
