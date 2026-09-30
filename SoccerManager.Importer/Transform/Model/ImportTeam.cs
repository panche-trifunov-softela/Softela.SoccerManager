using SoccerManager.Domain.Enums;

namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// An imported club.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt club id.</param>
/// <param name="Name">The club's trimmed name.</param>
/// <param name="LeagueId">The Transfermarkt domestic competition code the club plays in.</param>
/// <param name="FinancialState">The club's classified financial standing.</param>
/// <param name="StadiumKey">The key of the club's stadium in <see cref="ImportModel.Stadiums"/>, or <see langword="null"/> when it has none.</param>
/// <param name="SquadValue">The sum of the values of the club's imported players, in EUR.</param>
/// <param name="NetTransferRecord">The club's parsed net transfer record in EUR, or <see langword="null"/> when it could not be parsed.</param>
public sealed record ImportTeam(
    int TransfermarktId,
    string Name,
    string LeagueId,
    FinancialState FinancialState,
    string? StadiumKey,
    decimal SquadValue,
    decimal? NetTransferRecord);
