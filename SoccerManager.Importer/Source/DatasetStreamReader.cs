using System.Diagnostics;
using System.Net;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SoccerManager.Importer.Csv;

namespace SoccerManager.Importer.Source;

/// <summary>
/// Streams a single dataset table from the snapshot source straight into an in-memory accumulator, retrying
/// transient failures with a fresh accumulator each time, since a gzip stream cannot resume mid-way.
/// </summary>
public sealed class DatasetStreamReader : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly SourceOptions _options;
    private readonly ILogger<DatasetStreamReader> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatasetStreamReader"/> class.
    /// </summary>
    /// <param name="options">The source base URL and retry configuration.</param>
    /// <param name="logger">The logger used to report retries.</param>
    public DatasetStreamReader(IOptions<SourceOptions> options, ILogger<DatasetStreamReader> logger)
    {
        _options = options.Value;
        _logger = logger;
        _httpClient = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };
    }

    /// <summary>
    /// Streams the given table, folding every row into a fresh accumulator via <paramref name="addRow"/>. A
    /// transient failure (a connection cut, a server error, or the per-table timeout elapsing) discards that
    /// attempt's accumulator and retries with a new one, up to <see cref="SourceOptions.MaxAttempts"/> times.
    /// </summary>
    /// <typeparam name="T">The accumulator type the table's rows are folded into.</typeparam>
    /// <param name="table">The dataset table name, e.g. "players".</param>
    /// <param name="createAccumulator">Creates a fresh, empty accumulator for one attempt.</param>
    /// <param name="addRow">Folds one CSV record into the accumulator.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the read. A cancellation coming from this
    /// token is never treated as a retryable failure: it surfaces immediately as an <see cref="OperationCanceledException"/>
    /// carrying this token, even when tearing the connection down to unblock a stalled read raised a different
    /// exception underneath.</param>
    /// <returns>The accumulator built by the successful attempt, together with that attempt's read result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the table has no header row, or its header is missing a required column. Never retried.</exception>
    /// <exception cref="TimeoutException">Thrown when the per-table timeout elapses on the final attempt.</exception>
    public async Task<(T Accumulator, TableReadResult Result)> ReadAsync<T>(
        string table,
        Func<T> createAccumulator,
        Action<T, IReaderRow> addRow,
        CancellationToken cancellationToken)
    {
        for (var attempt = 1; ; attempt++)
        {
            using var tableTimeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            tableTimeoutSource.CancelAfter(TimeSpan.FromMinutes(_options.TableTimeoutMinutes));

            try
            {
                return await ReadOnceAsync(table, createAccumulator, addRow, tableTimeoutSource.Token).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    // Tearing the connection down (below) to unblock a stalled synchronous read surfaces as an I/O
                    // or disposed-object error rather than a cancellation; report it as the cancellation it is. An
                    // exception that is already an OperationCanceledException needs no rewrapping.
                    if (exception is OperationCanceledException)
                    {
                        throw;
                    }

                    throw new OperationCanceledException("The import was cancelled.", exception, cancellationToken);
                }

                // tableTimeoutSource is linked to cancellationToken, so it is also cancelled when the caller
                // cancels; that case is handled above, so reaching here with it cancelled means the per-table
                // timeout elapsed instead.
                var tableTimedOut = tableTimeoutSource.IsCancellationRequested;

                if (attempt < _options.MaxAttempts && (tableTimedOut || IsTransient(exception)))
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));

                    if (tableTimedOut)
                    {
                        _logger.LogWarning(exception, "Attempt {Attempt} to read {Table} timed out, retrying in {Delay}.", attempt, table, delay);
                    }
                    else
                    {
                        _logger.LogWarning(exception, "Attempt {Attempt} to read {Table} failed, retrying in {Delay}.", attempt, table, delay);
                    }

                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                    continue;
                }

                if (tableTimedOut)
                {
                    throw new TimeoutException($"Reading table '{table}' took longer than {_options.TableTimeoutMinutes} minute(s).", exception);
                }

                throw;
            }
        }
    }

    private async Task<(T Accumulator, TableReadResult Result)> ReadOnceAsync<T>(
        string table,
        Func<T> createAccumulator,
        Action<T, IReaderRow> addRow,
        CancellationToken tableToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var url = $"{_options.BaseUrl}{table}.csv.gz";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tableToken).ConfigureAwait(false);

        // The token just passed to SendAsync only covers receiving the headers, not the body reads that follow;
        // CsvHelper then reads the body synchronously, which cancellation cannot interrupt on its own. Registering
        // here tears the connection down on cancellation (the per-table timeout or Ctrl+C alike), so a blocked read
        // ends instead of hanging.
        using var abortOnCancel = tableToken.Register(static state => ((HttpResponseMessage)state!).Dispose(), response);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStreamAsync(tableToken).ConfigureAwait(false);
        var hashingStream = new HashingReadStream(body);
        using var csvReader = GzipCsvReader.Open(hashingStream);

        if (!csvReader.Read())
        {
            throw new InvalidOperationException($"Table '{table}' returned no header row.");
        }

        csvReader.ReadHeader();
        ValidateHeader(table, csvReader.HeaderRecord);

        var accumulator = createAccumulator();
        var rowsRead = 0;

        while (csvReader.Read())
        {
            tableToken.ThrowIfCancellationRequested();
            addRow(accumulator, csvReader);
            rowsRead++;
        }

        stopwatch.Stop();

        var result = new TableReadResult(table, hashingStream.BytesRead, rowsRead, hashingStream.GetHexHash(), stopwatch.Elapsed);

        return (accumulator, result);
    }

    private static void ValidateHeader(string table, string[]? header)
    {
        var present = new HashSet<string>(header ?? Array.Empty<string>(), StringComparer.Ordinal);
        var missing = DatasetTables.RequiredColumns[table].Where(column => !present.Contains(column)).ToArray();

        if (missing.Length > 0)
        {
            throw new InvalidOperationException($"Table '{table}' is missing required column(s): {string.Join(", ", missing)}.");
        }
    }

    private static bool IsTransient(Exception exception)
    {
        return exception switch
        {
            HttpRequestException { StatusCode: null } => true,
            HttpRequestException { StatusCode: HttpStatusCode status } when (int)status >= 500 => true,
            HttpIOException => true,
            TimeoutException => true,
            OperationCanceledException => true,
            _ => false,
        };
    }

    /// <summary>
    /// Releases the HTTP client this reader owns.
    /// </summary>
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
