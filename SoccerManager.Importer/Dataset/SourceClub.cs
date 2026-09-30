namespace SoccerManager.Importer.Dataset;

/// <summary>
/// An in-scope club, as read from the clubs table.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt club id.</param>
/// <param name="Name">The club's name, as recorded on Transfermarkt.</param>
/// <param name="LeagueId">The Transfermarkt domestic competition code the club plays in.</param>
/// <param name="StadiumName">The club's stadium name, or <see langword="null"/> when blank.</param>
/// <param name="StadiumSeats">The club's stadium capacity, or <see langword="null"/> when blank.</param>
/// <param name="NetTransferRecord">The club's raw net transfer record text, or <see langword="null"/> when blank.</param>
public sealed record SourceClub(
    int TransfermarktId,
    string Name,
    string LeagueId,
    string? StadiumName,
    int? StadiumSeats,
    string? NetTransferRecord);
