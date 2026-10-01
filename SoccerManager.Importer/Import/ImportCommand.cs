using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SoccerManager.Importer.Cli;
using SoccerManager.Importer.Database;
using SoccerManager.Importer.Database.Changes;
using SoccerManager.Importer.Database.Writing;
using SoccerManager.Importer.Dataset;
using SoccerManager.Importer.Report;
using SoccerManager.Importer.Transform;
using SoccerManager.Importer.Wikidata;

namespace SoccerManager.Importer.Import;

/// <summary>
/// Runs the import step: validates the run, streams and folds the dataset, enriches in-scope players from
/// Wikidata unless skipped, builds the import model, compares it against the database when one is configured, and
/// either reports what would change (a dry run) or writes the changes and reports what did.
/// </summary>
public sealed class ImportCommand
{
    private readonly ImportOptions _importOptions;
    private readonly DatabaseSettings _databaseSettings;
    private readonly WikidataOptions _wikidataOptions;
    private readonly DatasetLoader _datasetLoader;
    private readonly WikidataClient _wikidataClient;
    private readonly ImportModelBuilder _importModelBuilder;
    private readonly DatabaseSnapshotReader _databaseSnapshotReader;
    private readonly ImportPlanner _importPlanner;
    private readonly ImportWriter _importWriter;
    private readonly DryRunReport _dryRunReport;
    private readonly DatabaseChangesReport _databaseChangesReport;
    private readonly WriteResultReport _writeResultReport;
    private readonly ILogger<ImportCommand> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportCommand"/> class.
    /// </summary>
    /// <param name="importOptions">The import run's own configuration, including its write parallelism.</param>
    /// <param name="databaseSettings">Whether ConnectionStrings:soccermanager is actually configured for this run.</param>
    /// <param name="wikidataOptions">The Wikidata enrichment configuration.</param>
    /// <param name="datasetLoader">Streams and folds the dataset snapshot's tables.</param>
    /// <param name="wikidataClient">Queries Wikidata for the in-scope players.</param>
    /// <param name="importModelBuilder">Builds the import model from the streamed dataset and its Wikidata enrichment.</param>
    /// <param name="databaseSnapshotReader">Checks the database connection and reads the snapshot the import plan is built against.</param>
    /// <param name="importPlanner">Builds the import plan from the import model and the database snapshot.</param>
    /// <param name="importWriter">Writes the import plan to the database.</param>
    /// <param name="dryRunReport">Writes the import model's own review sections.</param>
    /// <param name="databaseChangesReport">Writes the database changes section for the import plan.</param>
    /// <param name="writeResultReport">Writes the write results section for a completed write.</param>
    /// <param name="logger">The logger used to report validation failures and progress.</param>
    public ImportCommand(
        IOptions<ImportOptions> importOptions,
        DatabaseSettings databaseSettings,
        IOptions<WikidataOptions> wikidataOptions,
        DatasetLoader datasetLoader,
        WikidataClient wikidataClient,
        ImportModelBuilder importModelBuilder,
        DatabaseSnapshotReader databaseSnapshotReader,
        ImportPlanner importPlanner,
        ImportWriter importWriter,
        DryRunReport dryRunReport,
        DatabaseChangesReport databaseChangesReport,
        WriteResultReport writeResultReport,
        ILogger<ImportCommand> logger)
    {
        _importOptions = importOptions.Value;
        _databaseSettings = databaseSettings;
        _wikidataOptions = wikidataOptions.Value;
        _datasetLoader = datasetLoader;
        _wikidataClient = wikidataClient;
        _importModelBuilder = importModelBuilder;
        _databaseSnapshotReader = databaseSnapshotReader;
        _importPlanner = importPlanner;
        _importWriter = importWriter;
        _dryRunReport = dryRunReport;
        _databaseChangesReport = databaseChangesReport;
        _writeResultReport = writeResultReport;
        _logger = logger;
    }

    /// <summary>
    /// Validates the run, streams and folds the dataset, enriches from Wikidata unless skipped, builds the import
    /// model, and either reports what a write would change (a dry run) or writes those changes to the database
    /// and reports what was written.
    /// </summary>
    /// <param name="dryRun">Whether the run writes only a review report and no database rows.</param>
    /// <param name="skipWikidata">Whether the Wikidata enrichment step should be skipped.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the run.</param>
    /// <returns>
    /// <see cref="ExitCodes.Success"/> when every planned row was written (or, for a dry run, on completion);
    /// <see cref="ExitCodes.Usage"/> for a usage error, such as a real run without a configured database, an
    /// invalid write parallelism, or the Wikidata step running without a configured user agent;
    /// <see cref="ExitCodes.Failure"/> when any row failed or was skipped, or on any other unhandled failure.
    /// </returns>
    public async Task<int> RunAsync(bool dryRun, bool skipWikidata, CancellationToken cancellationToken)
    {
        if (_importOptions.MaxDegreeOfParallelism < 1)
        {
            Console.Error.WriteLine("Import:MaxDegreeOfParallelism must be at least 1.");
            return ExitCodes.Usage;
        }

        if (!dryRun && !_databaseSettings.IsConfigured)
        {
            Console.Error.WriteLine(
                "Writing to the database needs ConnectionStrings:soccermanager (e.g. the ConnectionStrings__soccermanager " +
                "environment variable); or run with --dry-run to review without a database.");

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
            // A bad connection string or an unreachable server fails here, in seconds, rather than after the
            // dataset has been downloaded and Wikidata queried.
            if (_databaseSettings.IsConfigured)
            {
                await _databaseSnapshotReader.CheckConnectionAsync(cancellationToken).ConfigureAwait(false);
            }

            var dataset = await _datasetLoader.LoadAsync(cancellationToken).ConfigureAwait(false);

            var wikidata = skipWikidata
                ? null
                : await RunWikidataStepAsync(dataset, cancellationToken).ConfigureAwait(false);

            var model = _importModelBuilder.Build(dataset, wikidata);

            var plan = _databaseSettings.IsConfigured
                ? _importPlanner.Plan(model, await _databaseSnapshotReader.ReadAsync(model, cancellationToken).ConfigureAwait(false))
                : null;

            _dryRunReport.Write(Console.Out, model);
            _databaseChangesReport.Write(Console.Out, plan);

            if (dryRun)
            {
                return ExitCodes.Success;
            }

            // The usage guard above already refused a real run with no configured database, so a plan always
            // exists here.
            var writeResult = await _importWriter.WriteAsync(plan!, cancellationToken).ConfigureAwait(false);
            _writeResultReport.Write(Console.Out, writeResult);

            if (writeResult.NotWrittenCount > 0)
            {
                _logger.LogWarning("{Count} row(s) were not written.", writeResult.NotWrittenCount);
                return ExitCodes.Failure;
            }

            return ExitCodes.Success;
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
}
