using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SoccerManager.Application.Core.Command;
using SoccerManager.Importer.Database.Changes;

namespace SoccerManager.Importer.Database.Writing;

/// <summary>
/// Writes an import plan to the database, one table at a time, each table's rows written in parallel through
/// their own dependency injection scope.
/// </summary>
public sealed class ImportWriter
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ImportOptions _options;
    private readonly ILogger<ImportWriter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportWriter"/> class.
    /// </summary>
    /// <param name="scopeFactory">Creates the per-row dependency injection scopes each write dispatches through.</param>
    /// <param name="options">The import run's configuration, including its write parallelism.</param>
    /// <param name="logger">The logger used to report progress, failures and each table's summary.</param>
    public ImportWriter(IServiceScopeFactory scopeFactory, IOptions<ImportOptions> options, ILogger<ImportWriter> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Writes every table's planned rows, finishing one table before the next starts.
    /// </summary>
    /// <param name="plan">The plan to write.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the write.</param>
    /// <returns>The full write result.</returns>
    public async Task<ImportWriteResult> WriteAsync(ImportPlan plan, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var entityResults = new List<EntityWriteResult>();
        var failures = new ConcurrentQueue<WriteFailure>();

        foreach (var entityPlan in plan.Entities)
        {
            var entityStopwatch = Stopwatch.StartNew();

            if (entityPlan.Writes.Count == 0)
            {
                entityResults.Add(new EntityWriteResult(entityPlan.Entity, 0, 0, 0, 0, entityStopwatch.Elapsed));
                continue;
            }

            _logger.LogInformation(
                "{Entity}: writing {Count} rows ({Creates} creates, {Updates} updates).",
                entityPlan.Entity, entityPlan.Writes.Count, entityPlan.Creates, entityPlan.Updates);

            var created = 0;
            var updated = 0;
            var failed = 0;
            var skipped = 0;
            var processed = 0;

            await Parallel.ForEachAsync(
                entityPlan.Writes,
                new ParallelOptions { MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism, CancellationToken = cancellationToken },
                async (row, token) =>
                {
                    // One scope per row: each scope gets its own UnitOfWork and connection, so the parallel writes
                    // never share a transaction.
                    await using var scope = _scopeFactory.CreateAsyncScope();
                    var dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();

                    void RecordFailure(string reason)
                    {
                        Interlocked.Increment(ref failed);
                        failures.Enqueue(new WriteFailure(entityPlan.Entity, row.Key, row.Name, reason));
                        _logger.LogWarning("{Entity} {Key} ({Name}) failed: {Reason}", entityPlan.Entity, row.Key, row.Name, reason);
                    }

                    try
                    {
                        var outcome = await row.ExecuteAsync(new WriteContext(dispatcher, plan.Ids, token)).ConfigureAwait(false);

                        if (outcome.IsSkipped)
                        {
                            Interlocked.Increment(ref skipped);
                            failures.Enqueue(new WriteFailure(entityPlan.Entity, row.Key, row.Name, outcome.SkipReason!));
                            _logger.LogWarning("{Entity} {Key} ({Name}) skipped: {Reason}", entityPlan.Entity, row.Key, row.Name, outcome.SkipReason);
                        }
                        else if (row.Kind == ChangeKind.Create)
                        {
                            Interlocked.Increment(ref created);
                        }
                        else
                        {
                            Interlocked.Increment(ref updated);
                        }
                    }
                    catch (OperationCanceledException) when (token.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (FluentValidation.ValidationException validationException)
                    {
                        RecordFailure("Validation failed: " + string.Join("; ", validationException.Errors.Select(error => error.ErrorMessage)));
                    }
                    catch (KeyNotFoundException keyNotFoundException)
                    {
                        RecordFailure(keyNotFoundException.Message);
                    }
                    catch (System.Data.Common.DbException dbException)
                    {
                        RecordFailure("Database error: " + dbException.Message);
                    }
                    catch (Exception exception)
                    {
                        RecordFailure($"{exception.GetType().Name}: {exception.Message}");
                    }

                    var done = Interlocked.Increment(ref processed);
                    if (done % 1000 == 0)
                    {
                        _logger.LogInformation("{Entity}: {Done} of {Total} rows written.", entityPlan.Entity, done, entityPlan.Writes.Count);
                    }
                }).ConfigureAwait(false);

            entityStopwatch.Stop();
            entityResults.Add(new EntityWriteResult(entityPlan.Entity, created, updated, failed, skipped, entityStopwatch.Elapsed));

            _logger.LogInformation(
                "{Entity}: {Created} created, {Updated} updated, {Failed} failed, {Skipped} skipped in {Elapsed}.",
                entityPlan.Entity, created, updated, failed, skipped, entityStopwatch.Elapsed);
        }

        stopwatch.Stop();

        return new ImportWriteResult(entityResults, failures.ToArray(), stopwatch.Elapsed);
    }
}
