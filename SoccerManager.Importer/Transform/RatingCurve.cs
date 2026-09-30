namespace SoccerManager.Importer.Transform;

/// <summary>
/// Named anchors of the piecewise-linear curve that maps a player's composite percentile (0..1) to a 1-100 rating.
/// </summary>
public sealed class RatingCurve
{
    /// <summary>The rating at the 0th percentile.</summary>
    public double P0 { get; set; } = 50;

    /// <summary>The rating at the 10th percentile.</summary>
    public double P10 { get; set; } = 60;

    /// <summary>The rating at the 50th percentile.</summary>
    public double P50 { get; set; } = 70;

    /// <summary>The rating at the 90th percentile.</summary>
    public double P90 { get; set; } = 80;

    /// <summary>The rating at the 99th percentile.</summary>
    public double P99 { get; set; } = 88;

    /// <summary>The rating at the 100th percentile.</summary>
    public double P100 { get; set; } = 93;
}
