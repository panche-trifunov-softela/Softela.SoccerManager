namespace SoccerManager.Importer.Transform;

/// <summary>
/// Configuration for the transform step that is not specific to any one calculator.
/// </summary>
public sealed class TransformOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Transform";

    /// <summary>
    /// The snapshot's as-of date. A player's age, and the plausibility of a resolved date of birth, are computed
    /// against this date rather than the run's own clock, since the underlying dataset is frozen.
    /// </summary>
    public DateOnly ReferenceDate { get; set; } = new(2026, 7, 1);
}
