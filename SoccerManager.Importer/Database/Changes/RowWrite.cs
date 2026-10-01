namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// One planned row write: a create or an update, ready to be dispatched once a dependency injection scope and
/// command dispatcher are available.
/// </summary>
/// <param name="Kind">Whether this row creates a new row or updates an existing one.</param>
/// <param name="Key">
/// The row's natural-key display key: "TM &lt;id&gt;" for a player, team or national team, the name for a stadium,
/// position or referee, or "TM &lt;id&gt; &lt;position name&gt;" for a player position.
/// </param>
/// <param name="Name">The row's display name, for the report.</param>
/// <param name="Changes">The fields an update would change. Always empty for a create.</param>
/// <param name="ExecuteAsync">
/// Builds the row's typed request at write time, resolving any link through <see cref="WriteContext.Ids"/>,
/// dispatches it, and, for a create, registers the new id in <see cref="WriteContext.Ids"/>.
/// </param>
public sealed record RowWrite(
    ChangeKind Kind,
    string Key,
    string Name,
    IReadOnlyList<FieldChange> Changes,
    Func<WriteContext, Task<WriteOutcome>> ExecuteAsync);
