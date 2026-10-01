namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// The planned writes for one table, with how many existing rows were left unchanged and any notes about how the
/// plan was built.
/// </summary>
/// <param name="Entity">The table's display name, e.g. "Stadiums" or "Player positions".</param>
/// <param name="Writes">The rows to create or update, in no particular order within the table.</param>
/// <param name="Unchanged">The number of existing rows that matched the import data exactly and need no write.</param>
/// <param name="Notes">Notes about the plan: a name key matching several existing rows, an existing row that was kept, or a mismatch left as is.</param>
public sealed record EntityPlan(string Entity, IReadOnlyList<RowWrite> Writes, int Unchanged, IReadOnlyList<string> Notes)
{
    /// <summary>The number of rows this plan would create.</summary>
    public int Creates => Writes.Count(write => write.Kind == ChangeKind.Create);

    /// <summary>The number of rows this plan would update.</summary>
    public int Updates => Writes.Count(write => write.Kind == ChangeKind.Update);
}
