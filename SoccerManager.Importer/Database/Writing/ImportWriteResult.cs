namespace SoccerManager.Importer.Database.Writing;

/// <summary>
/// The full result of writing an import plan to the database.
/// </summary>
/// <param name="Entities">Each table's write result, in write order.</param>
/// <param name="Failures">Every row that was not written, whether it failed or was skipped.</param>
/// <param name="Elapsed">How long the whole write took.</param>
public sealed record ImportWriteResult(IReadOnlyList<EntityWriteResult> Entities, IReadOnlyList<WriteFailure> Failures, TimeSpan Elapsed)
{
    /// <summary>The total number of rows that were not written, whether they failed or were skipped.</summary>
    public int NotWrittenCount => Failures.Count;
}
