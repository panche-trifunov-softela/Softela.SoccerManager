using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SoccerManager.Importer.Cli;
using SoccerManager.Importer.Dataset;
using SoccerManager.Importer.Report;
using SoccerManager.Importer.Transform;
using SoccerManager.Importer.Wikidata;

namespace SoccerManager.Importer.Import;

/// <summary>
/// Runs the import step: validates the run, streams and folds the dataset, enriches in-scope players from
/// Wikidata unless skipped, builds the import model, and hands it to the report step.
/// </summary>
public sealed class ImportCommand
{
    private readonly WikidataOptions _wikidataOptions;
    private readonly DatasetLoader _datasetLoader;
    private readonly WikidataClient _wikidataClient;
    private readonly ImportModelBuilder _importModelBuilder;
    private readonly DryRunReport _dryRunReport;
    private readonly ILogger<ImportCommand> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportCommand"/> class.
    /// </summary>
    /// <param name="wikidataOptions">The Wikidata enrichment configuration.</param>
    /// <param name="datasetLoader">Streams and folds the dataset snapshot's tables.</param>
    /// <param name="wikidataClient">Queries Wikidata for the in-scope players.</param>
    /// <param name="importModelBuilder">Builds the import model from the streamed dataset and its Wikidata enrichment.</param>
    /// <param name="dryRunReport">Writes the review report for the built import model.</param>
    /// <param name="logger">The logger used to report validation failures and progress.</param>
    public ImportCommand(
        IOptions<WikidataOptions> wikidataOptions,
        DatasetLoader datasetLoader,
        WikidataClient wikidataClient,
        ImportModelBuilder importModelBuilder,
        DryRunReport dryRunReport,
        ILogger<ImportCommand> logger)
    {
        _wikidataOptions = wikidataOptions.Value;
        _datasetLoader = datasetLoader;
        _wikidataClient = wikidataClient;
        _importModelBuilder = importModelBuilder;
        _dryRunReport = dryRunReport;
        _logger = logger;
    }

    /// <summary>
    /// Validates the run, streams and folds the dataset, enriches from Wikidata unless skipped, and reports the
    /// result. Database writes are not yet implemented, so a run without <paramref name="dryRun"/> is refused.
    /// </summary>
    /// <param name="dryRun">Whether the run writes only a review report and no database rows.</param>
    /// <param name="skipWikidata">Whether the Wikidata enrichment step should be skipped.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the run.</param>
    /// <returns>
    /// <see cref="ExitCodes.Success"/> on completion, <see cref="ExitCodes.Usage"/> when <paramref name="dryRun"/>
    /// is <see langword="false"/> or the Wikidata step would run without a configured user agent, or
    /// <see cref="ExitCodes.Failure"/> on any other failure.
    /// </returns>
    public async Task<int> RunAsync(bool dryRun, bool skipWikidata, CancellationToken cancellationToken)
    {
        if (!dryRun)
        {
            Console.Error.WriteLine("Writing to the database arrives in the next change; run with --dry-run.");
            return ExitCodes.Usage;
        }

        if (!skipWikidata && string.IsNullOrWhiteSpace(_wikidataOptions.UserAgent))
        {
            _logger.LogError(
                "Wikidata:UserAgent must be set (or pass --skip-wikidata). The Wikimedia User-Agent policy requires a client name, version and contact.");

            return ExitCodes.Usage;
        }

        try
        {
            var dataset = await _datasetLoader.LoadAsync(cancellationToken).ConfigureAwait(false);

            var wikidata = skipWikidata
                ? null
                : await RunWikidataStepAsync(dataset, cancellationToken).ConfigureAwait(false);

            return ReportOutcome(dataset, wikidata);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Import failed.");
            return ExitCodes.Failure;
        }
    }

    private async Task<IReadOnlyDictionary<int, WikidataPlayer>> RunWikidataStepAsync(Dataset.Dataset dataset, CancellationToken cancellationToken)
    {
        var transfermarktIds = dataset.Players.Select(player => player.TransfermarktId).ToArray();

        var stopwatch = Stopwatch.StartNew();
        var matches = await _wikidataClient.QueryPlayersAsync(transfermarktIds, cancellationToken).ConfigureAwait(false);
        stopwatch.Stop();

        _logger.LogInformation(
            "Wikidata: {Matched} of {Requested} players matched in {Elapsed}.",
            matches.Count, transfermarktIds.Length, stopwatch.Elapsed);

        return matches.ToDictionary(player => player.TransfermarktId);
    }

    /// <summary>
    /// Builds the import model from <paramref name="dataset"/> and <paramref name="wikidata"/>, then prints the
    /// full review report to <see cref="Console.Out"/>:
    /// <c>var model = _importModelBuilder.Build(dataset, wikidata); _dryRunReport.Write(Console.Out, model); return ExitCodes.Success;</c>.
    /// Nothing is written to disk; database writes arrive in a later change.
    /// </summary>
    /// <param name="dataset">The streamed and folded dataset.</param>
    /// <param name="wikidata">The Wikidata matches by Transfermarkt id, or <see langword="null"/> when the step was skipped.</param>
    /// <returns><see cref="ExitCodes.Success"/>.</returns>
    private int ReportOutcome(Dataset.Dataset dataset, IReadOnlyDictionary<int, WikidataPlayer>? wikidata)
    {
        var model = _importModelBuilder.Build(dataset, wikidata);
        _dryRunReport.Write(Console.Out, model);
        return ExitCodes.Success;
    }
}
