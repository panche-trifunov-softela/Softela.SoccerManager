using SoccerManager.Application.Dtos;

namespace SoccerManager.Importer.Database;

/// <summary>
/// A snapshot of the database rows the import plan is compared against.
/// </summary>
/// <param name="Stadiums">Every stadium in the database.</param>
/// <param name="Positions">Every position in the database.</param>
/// <param name="Referees">Every referee in the database.</param>
/// <param name="Teams">Every team in the database.</param>
/// <param name="NationalTeams">Every national team in the database.</param>
/// <param name="Players">Every player in the database.</param>
/// <param name="PlayerPositionsByPlayerId">
/// The position ratings of every database player whose Transfermarkt id is also in the import model, keyed by the
/// database player id. A player the import model does not reference has no entry.
/// </param>
public sealed record DatabaseSnapshot(
    IReadOnlyList<StadiumDto> Stadiums,
    IReadOnlyList<PositionDto> Positions,
    IReadOnlyList<RefereeDto> Referees,
    IReadOnlyList<TeamDto> Teams,
    IReadOnlyList<NationalTeamDto> NationalTeams,
    IReadOnlyList<PlayerDto> Players,
    IReadOnlyDictionary<int, IReadOnlyList<PlayerPositionDto>> PlayerPositionsByPlayerId);
