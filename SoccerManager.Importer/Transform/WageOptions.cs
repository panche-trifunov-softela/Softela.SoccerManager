namespace SoccerManager.Importer.Transform;

/// <summary>
/// Configuration for estimating a player's weekly wage from their value.
/// </summary>
public sealed class WageOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Transform:Wage";

    /// <summary>The multiplier in the weekly wage curve <c>A * value^B / 52</c>.</summary>
    public double A { get; set; } = 18.6;

    /// <summary>The exponent in the weekly wage curve <c>A * value^B / 52</c>.</summary>
    public double B { get; set; } = 0.739;

    /// <summary>The minimum weekly wage (EUR) any player is estimated to earn.</summary>
    public decimal WeeklyFloor { get; set; } = 1000m;
}
