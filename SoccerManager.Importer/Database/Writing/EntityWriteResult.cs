namespace SoccerManager.Importer.Database.Writing;

/// <summary>
/// How one table's planned writes turned out.
/// </summary>
/// <param name="Entity">The table's display name.</param>
/// <param name="Created">The number of rows created.</param>
/// <param name="Updated">The number of rows updated.</param>
/// <param name="Failed">The number of rows whose command threw and was logged and skipped.</param>
/// <param name="Skipped">The number of rows skipped because a row they depend on was not itself written.</param>
/// <param name="Elapsed">How long this table's writes took.</param>
public sealed record EntityWriteResult(string Entity, int Created, int Updated, int Failed, int Skipped, TimeSpan Elapsed);
