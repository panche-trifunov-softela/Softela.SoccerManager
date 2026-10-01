namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// The outcome of writing one planned row, short of an exception: either written, or skipped for a stated reason
/// (most often that a row it depends on was not itself written).
/// </summary>
/// <param name="SkipReason">Why the row was skipped, or <see langword="null"/> when it was written.</param>
public sealed record WriteOutcome(string? SkipReason)
{
    /// <summary>The row was written.</summary>
    public static WriteOutcome Written { get; } = new((string?)null);

    /// <summary>Builds the outcome for a row that was skipped instead of written.</summary>
    /// <param name="reason">Why the row was skipped.</param>
    /// <returns>The skipped outcome.</returns>
    public static WriteOutcome Skipped(string reason) => new(reason);

    /// <summary>Whether this outcome represents a skipped row.</summary>
    public bool IsSkipped => SkipReason is not null;
}
