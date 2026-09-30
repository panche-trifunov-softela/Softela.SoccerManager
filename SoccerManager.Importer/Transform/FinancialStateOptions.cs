namespace SoccerManager.Importer.Transform;

/// <summary>
/// Configuration for classifying a club's overall financial standing from its squad value and net transfer record.
/// </summary>
public sealed class FinancialStateOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Transform:FinancialState";

    /// <summary>The squad value (EUR) at or above which a club is classified as very rich.</summary>
    public decimal VeryRichMin { get; set; } = 600_000_000m;

    /// <summary>The squad value (EUR) at or above which a club is classified as rich.</summary>
    public decimal RichMin { get; set; } = 250_000_000m;

    /// <summary>The squad value (EUR) at or above which a club is classified as average.</summary>
    public decimal AverageMin { get; set; } = 100_000_000m;

    /// <summary>The squad value (EUR) at or above which a club is classified as poor, below which it is very poor.</summary>
    public decimal PoorMin { get; set; } = 40_000_000m;

    /// <summary>The net spend (EUR) a club's negative net transfer record must reach to move its classification up one level.</summary>
    public decimal NetSpendStepUp { get; set; } = 100_000_000m;
}
