namespace SoccerManager.Importer.Source;

/// <summary>
/// The outcome of streaming and validating one dataset table.
/// </summary>
/// <param name="Table">The table name, e.g. "players".</param>
/// <param name="Bytes">The number of compressed bytes read from the response body.</param>
/// <param name="RowsRead">The number of data rows read from the table.</param>
/// <param name="Sha256">The SHA-256 hash of the compressed response body, as lowercase hex.</param>
/// <param name="Duration">How long the successful attempt took, from the request being sent to the last row being read.</param>
public sealed record TableReadResult(string Table, long Bytes, int RowsRead, string Sha256, TimeSpan Duration);
