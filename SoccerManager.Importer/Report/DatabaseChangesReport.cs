using System.Globalization;
using SoccerManager.Importer.Database;
using SoccerManager.Importer.Database.Changes;

namespace SoccerManager.Importer.Report;

/// <summary>
/// Writes the database changes section of the report: what an import run would create, update or leave unchanged,
/// compared against the database snapshot read for this run. Nothing here writes to the database.
/// </summary>
public sealed class DatabaseChangesReport
{
    /// <summary>
    /// Writes the "10. Database changes" section to <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The writer the section is printed to.</param>
    /// <param name="plan">The import plan, or <see langword="null"/> when the comparison was skipped because ConnectionStrings:soccermanager is not set.</param>
    public void Write(TextWriter writer, ImportPlan? plan)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteLine("== 10. Database changes ==");

        if (plan is null)
        {
            writer.WriteLine("Skipped: ConnectionStrings:soccermanager is not set.");
            writer.WriteLine();
            return;
        }

        writer.WriteLine($"Rows this run writes are stamped CreatedBy/ModifiedBy: {ImportCurrentUser.ImportUserId}");
        writer.WriteLine();

        writer.WriteLine(ReportTable.Row(
            ReportTable.Left("Table", 16),
            ReportTable.Right("Create", 8),
            ReportTable.Right("Update", 8),
            ReportTable.Right("Unchanged", 10)));

        foreach (var entity in plan.Entities)
        {
            writer.WriteLine(ReportTable.Row(
                ReportTable.Left(entity.Entity, 16),
                ReportTable.Right(ReportNumberFormat.Count(entity.Creates), 8),
                ReportTable.Right(ReportNumberFormat.Count(entity.Updates), 8),
                ReportTable.Right(ReportNumberFormat.Count(entity.Unchanged), 10)));
        }

        writer.WriteLine();

        WriteUpdateDetails(writer, plan);
        WriteNotes(writer, plan);
    }

    private static void WriteUpdateDetails(TextWriter writer, ImportPlan plan)
    {
        foreach (var entity in plan.Entities)
        {
            var updates = entity.Writes.Where(write => write.Kind == ChangeKind.Update).ToArray();
            if (updates.Length == 0)
            {
                continue;
            }

            writer.WriteLine($"{entity.Entity}:");

            var fieldCounts = updates
                .SelectMany(write => write.Changes)
                .GroupBy(change => change.Field, StringComparer.Ordinal)
                .OrderByDescending(group => group.Count());

            foreach (var fieldGroup in fieldCounts)
            {
                writer.WriteLine($"  {fieldGroup.Key}: {ReportNumberFormat.Count(fieldGroup.Count())}");
            }

            writer.WriteLine();

            writer.WriteLine("  Samples:");
            foreach (var write in updates.Take(10))
            {
                var fieldText = string.Join("; ", write.Changes.Select(change => $"{change.Field} {FormatValue(change.Before)} -> {FormatValue(change.After)}"));
                writer.WriteLine($"    {write.Name} ({write.Key}): {fieldText}");
            }

            writer.WriteLine();
        }
    }

    private static void WriteNotes(TextWriter writer, ImportPlan plan)
    {
        foreach (var entity in plan.Entities)
        {
            if (entity.Notes.Count == 0)
            {
                continue;
            }

            writer.WriteLine($"{entity.Entity} notes:");

            foreach (var note in entity.Notes.Take(20))
            {
                writer.WriteLine($"  {note}");
            }

            if (entity.Notes.Count > 20)
            {
                writer.WriteLine($"  ... and {entity.Notes.Count - 20} more");
            }

            writer.WriteLine();
        }
    }

    // Formats a field's typed before/after value for the report: money as euros, a date as yyyy-MM-dd, and a
    // missing value as "(none)" rather than an empty cell that could be mistaken for a blank.
    private static string FormatValue(object? value)
    {
        return value switch
        {
            null => "(none)",
            decimal money => ReportNumberFormat.Eur(money),
            DateOnly date => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            _ => value.ToString() ?? "(none)",
        };
    }
}
