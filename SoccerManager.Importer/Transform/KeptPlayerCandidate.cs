using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// A player that survived the name and date-of-birth drop checks, on its way to becoming an
/// <see cref="ImportPlayer"/>. Builder-internal: it is missing only the rating, which needs the full kept-player
/// cohort to compute, and its image URL may still be replaced by <see langword="null"/> once placeholder URLs
/// shared by too many players are detected.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt player id.</param>
/// <param name="Name">The player's trimmed name.</param>
/// <param name="DateOfBirth">The player's resolved date of birth.</param>
/// <param name="Value">The player's resolved value in EUR.</param>
/// <param name="Wage">The player's estimated weekly wage in EUR.</param>
/// <param name="ImageUrl">The player's image URL, or <see langword="null"/> when blank or longer than 500 characters.</param>
/// <param name="TeamTransfermarktId">The Transfermarkt id of the player's resolved current club, or <see langword="null"/>.</param>
/// <param name="NationalTeamTransfermarktId">The Transfermarkt id of the player's resolved current national team, or <see langword="null"/>.</param>
/// <param name="InternationalCaps">The player's international caps.</param>
/// <param name="Group">The position group the player's rating is computed relative to.</param>
/// <param name="Positions">The player's positions: the main one at quality 100, followed by any secondary ones.</param>
internal sealed record KeptPlayerCandidate(
    int TransfermarktId,
    string Name,
    DateOnly DateOfBirth,
    decimal Value,
    decimal Wage,
    string? ImageUrl,
    int? TeamTransfermarktId,
    int? NationalTeamTransfermarktId,
    int InternationalCaps,
    PositionGroup Group,
    IReadOnlyList<ImportPlayerPosition> Positions);
