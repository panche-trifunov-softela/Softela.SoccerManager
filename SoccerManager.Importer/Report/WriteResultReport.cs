using SoccerManager.Importer.Database.Writing;

namespace SoccerManager.Importer.Report;

/// <summary>
/// Writes the write results section of the report: what an import run actually created, updated, failed or
/// skipped once the writes have run.
/// </summary>
public sealed class WriteResultReport
{
    /// <summary>
    /// Writes the "11. Write results" section to <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The writer the section is printed to.</param>
    /// <param name="result">The write result to report on.</param>
    public void Write(TextWriter writer, ImportWriteResult result)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(result);

        writer.WriteLine("== 11. Write results ==");

        writer.WriteLine(ReportTable.Row(
            ReportTable.Left("Table", 16),
            ReportTable.Right("Created", 8),
            ReportTable.Right("Updated", 8),
            ReportTable.Right("Failed", 8),
            ReportTable.Right("Skipped", 8),
            ReportTable.Right("Time", 8)));

        var totalCreated = 0;
        var totalUpdated = 0;
        var totalFailed = 0;
        var totalSkipped = 0;

        foreach (var entity in result.Entities)
        {
            writer.WriteLine(ReportTable.Row(
                ReportTable.Left(entity.Entity, 16),
                ReportTable.Right(ReportNumberFormat.Count(entity.Created), 8),
                ReportTable.Right(ReportNumberFormat.Count(entity.Updated), 8),
                ReportTable.Right(ReportNumberFormat.Count(entity.Failed), 8),
                ReportTable.Right(ReportNumberFormat.Count(entity.Skipped), 8),
                ReportTable.Right(ReportNumberFormat.Seconds(entity.Elapsed), 8)));

            totalCreated += entity.Created;
            totalUpdated += entity.Updated;
            totalFailed += entity.Failed;
            totalSkipped += entity.Skipped;
        }

        writer.WriteLine(ReportTable.Row(
            ReportTable.Left("Total", 16),
            ReportTable.Right(ReportNumberFormat.Count(totalCreated), 8),
            ReportTable.Right(ReportNumberFormat.Count(totalUpdated), 8),
            ReportTable.Right(ReportNumberFormat.Count(totalFailed), 8),
            ReportTable.Right(ReportNumberFormat.Count(totalSkipped), 8),
            ReportTable.Right(ReportNumberFormat.Seconds(result.Elapsed), 8)));

        writer.WriteLine();

        if (result.Failures.Count == 0)
        {
            return;
        }

        writer.WriteLine($"Rows not written (first 50 of {ReportNumberFormat.Count(result.Failures.Count)}):");
        foreach (var failure in result.Failures.Take(50))
        {
            writer.WriteLine($"  {failure.Entity} | {failure.Key} | {failure.Name} | {failure.Reason}");
        }
    }
}
