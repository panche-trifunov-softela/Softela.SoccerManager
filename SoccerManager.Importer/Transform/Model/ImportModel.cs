namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// The full result of transforming a streamed <see cref="Dataset.Dataset"/>, ready to be reported or, in a later
/// change, written to the database.
/// </summary>
/// <param name="Positions">The fixed catalog of the 13 recognized positions.</param>
/// <param name="Stadiums">The de-duplicated stadiums built from the imported clubs.</param>
/// <param name="Teams">The imported clubs.</param>
/// <param name="NationalTeams">The national teams actually referenced by an imported player.</param>
/// <param name="Referees">The distinct referee names recorded on domestic league window games.</param>
/// <param name="Players">The imported players.</param>
/// <param name="Diagnostics">Everything the dry-run report needs beyond the entity lists above.</param>
public sealed record ImportModel(
    IReadOnlyList<ImportPosition> Positions,
    IReadOnlyList<ImportStadium> Stadiums,
    IReadOnlyList<ImportTeam> Teams,
    IReadOnlyList<ImportNationalTeam> NationalTeams,
    IReadOnlyList<ImportReferee> Referees,
    IReadOnlyList<ImportPlayer> Players,
    ImportDiagnostics Diagnostics);
