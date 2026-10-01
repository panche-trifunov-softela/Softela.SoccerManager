using SoccerManager.Application.Core.Command;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Everything a <see cref="RowWrite"/> needs to build and dispatch its command at write time.
/// </summary>
/// <param name="Dispatcher">Dispatches the row's command to its handler, scoped to this row's own dependency injection scope.</param>
/// <param name="Ids">The ids written so far by this run, so a link to another table's row can be resolved even when that row was only just created.</param>
/// <param name="CancellationToken">The token used to cancel the write.</param>
public sealed record WriteContext(ICommandDispatcher Dispatcher, ImportIds Ids, CancellationToken CancellationToken);
