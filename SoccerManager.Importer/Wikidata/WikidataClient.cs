using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SoccerManager.Importer.Wikidata;

/// <summary>
/// Queries the Wikidata SPARQL endpoint for the players in a harvest run's scope, one batch and one in-flight
/// request at a time, as the Wikimedia query service asks of its heavier clients.
/// </summary>
public sealed class WikidataClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly WikidataOptions _options;
    private readonly ILogger<WikidataClient> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WikidataClient"/> class.
    /// </summary>
    /// <param name="options">The Wikidata endpoint, batching and retry configuration.</param>
    /// <param name="logger">The logger used to report retries.</param>
    public WikidataClient(IOptions<WikidataOptions> options, ILogger<WikidataClient> logger)
    {
        _options = options.Value;
        _logger = logger;
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(_options.RequestTimeoutSeconds) };
    }

    /// <summary>
    /// Queries Wikidata for the given Transfermarkt player ids, one <see cref="WikidataOptions.BatchSize"/>-sized
    /// batch per request, sequentially.
    /// </summary>
    /// <param name="transfermarktIds">The Transfermarkt player ids to look up.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the query.</param>
    /// <returns>The Wikidata players matched across every batch.</returns>
    public async Task<IReadOnlyList<WikidataPlayer>> QueryPlayersAsync(IReadOnlyList<int> transfermarktIds, CancellationToken cancellationToken)
    {
        var players = new List<WikidataPlayer>();

        foreach (var batch in transfermarktIds.Chunk(_options.BatchSize))
        {
            var query = WikidataPlayerQuery.Build(batch);
            var json = await PostWithRetryAsync(query, cancellationToken).ConfigureAwait(false);

            players.AddRange(WikidataResultParser.Parse(json));
        }

        return players;
    }

    private async Task<string> PostWithRetryAsync(string query, CancellationToken cancellationToken)
    {
        for (var attempt = 1; ; attempt++)
        {
            HttpResponseMessage response;

            try
            {
                using var request = BuildRequest(query);
                response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception) when (attempt < _options.MaxAttempts && !cancellationToken.IsCancellationRequested && IsTransient(exception))
            {
                var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                _logger.LogWarning(exception, "Wikidata request failed, retrying in {Delay}.", delay);
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                continue;
            }

            using (response)
            {
                if (response.StatusCode == HttpStatusCode.TooManyRequests && attempt < _options.MaxAttempts)
                {
                    var delay = ParseRetryAfter(response.Headers.RetryAfter) ?? TimeSpan.FromSeconds(Math.Pow(2, attempt));
                    _logger.LogWarning("Wikidata rate limited (429), waiting {Delay} before retrying.", delay);
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                    continue;
                }

                if ((int)response.StatusCode >= 500 && attempt < _options.MaxAttempts)
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                    _logger.LogWarning("Wikidata request failed with {StatusCode}, retrying in {Delay}.", response.StatusCode, delay);
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                    continue;
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }

    private HttpRequestMessage BuildRequest(string query)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _options.Endpoint)
        {
            Content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("query", query) }),
        };

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/sparql-results+json"));
        request.Headers.UserAgent.ParseAdd(_options.UserAgent);

        return request;
    }

    private static TimeSpan? ParseRetryAfter(RetryConditionHeaderValue? retryAfter)
    {
        if (retryAfter is null)
        {
            return null;
        }

        if (retryAfter.Delta is { } delta)
        {
            return delta;
        }

        if (retryAfter.Date is { } date)
        {
            var remaining = date - DateTimeOffset.UtcNow;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }

        return null;
    }

    private static bool IsTransient(Exception exception)
    {
        return exception switch
        {
            HttpRequestException => true,
            TaskCanceledException => true,
            TimeoutException => true,
            HttpIOException => true,
            _ => false,
        };
    }

    /// <summary>
    /// Releases the HTTP client this client owns.
    /// </summary>
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
