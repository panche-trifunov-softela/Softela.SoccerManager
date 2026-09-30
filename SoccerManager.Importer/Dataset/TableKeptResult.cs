using SoccerManager.Importer.Source;

namespace SoccerManager.Importer.Dataset;

/// <summary>
/// One table's streaming result together with how many of its rows were kept after scope filtering.
/// </summary>
/// <param name="ReadResult">The raw streaming result: table name, bytes, rows read, hash and duration.</param>
/// <param name="RowsKept">The number of rows kept after applying the table's scope filter.</param>
public sealed record TableKeptResult(TableReadResult ReadResult, int RowsKept);
