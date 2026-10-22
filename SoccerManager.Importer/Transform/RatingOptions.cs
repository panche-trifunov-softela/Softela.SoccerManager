namespace SoccerManager.Importer.Transform;

/// <summary>
/// Configuration for the rating calculation (Rating v1). The league coefficients and the per-position-group weights
/// are code defaults only: unlike the scalar values below, they are not intended to be overridden from the command
/// line.
/// </summary>
public sealed class RatingOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Transform:Rating";

    /// <summary>The combined league and UEFA minutes a player must reach before the output component is scored at all.</summary>
    public double MinOutputMinutes { get; set; } = 450;

    /// <summary>The multiplier applied to UEFA goals and assists in the output component, relative to league ones.</summary>
    public double UefaOutputWeight { get; set; } = 1.2;

    /// <summary>The age below which the market value component receives a young-player premium.</summary>
    public double YoungPremiumAge { get; set; } = 23;

    /// <summary>The per-year size of the young-player premium applied below <see cref="YoungPremiumAge"/>.</summary>
    public double YoungPremiumPerYear { get; set; } = 0.08;

    /// <summary>
    /// The relative strength coefficient of each domestic league, keyed by its Transfermarkt competition code. Hand-set
    /// for GB1, ES1, IT1, L1, FR1, PO1 and NL1; the others are 0.330 + 0.111 * ln(V), fitted to those seven, where V
    /// is the league's mean total value, in EUR millions, of a club's 11 most valuable players (2026-07-06 dataset).
    /// </summary>
    public IDictionary<string, double> LeagueCoefficients { get; set; } = new Dictionary<string, double>(StringComparer.Ordinal)
    {
        ["GB1"] = 1.00,
        ["ES1"] = 0.96,
        ["IT1"] = 0.94,
        ["L1"] = 0.93,
        ["FR1"] = 0.88,
        ["BRA1"] = 0.81,
        ["PO1"] = 0.80,
        ["NL1"] = 0.80,
        ["BE1"] = 0.79,
        ["TR1"] = 0.78,
        ["SA1"] = 0.77,
        ["MLS1"] = 0.73,
        ["GR1"] = 0.70,
        ["ARG1"] = 0.69,
    };

    /// <summary>The component weights for goalkeepers.</summary>
    public RatingWeights Goalkeeper { get; set; } = new()
    {
        MarketValue = 0.35,
        Minutes = 0.30,
        Output = 0,
        Defence = 0.10,
        Team = 0.10,
        League = 0.10,
        Caps = 0.05,
    };

    /// <summary>The component weights for defenders.</summary>
    public RatingWeights Defender { get; set; } = new()
    {
        MarketValue = 0.35,
        Minutes = 0.25,
        Output = 0,
        Defence = 0.10,
        Team = 0.15,
        League = 0.10,
        Caps = 0.05,
    };

    /// <summary>The component weights for midfielders.</summary>
    public RatingWeights Midfield { get; set; } = new()
    {
        MarketValue = 0.35,
        Minutes = 0.25,
        Output = 0.10,
        Defence = 0,
        Team = 0.15,
        League = 0.10,
        Caps = 0.05,
    };

    /// <summary>The component weights for attackers.</summary>
    public RatingWeights Attack { get; set; } = new()
    {
        MarketValue = 0.35,
        Minutes = 0.15,
        Output = 0.20,
        Defence = 0,
        Team = 0.15,
        League = 0.10,
        Caps = 0.05,
    };

    /// <summary>The curve the composite score's percentile is mapped through to produce the final 1-100 rating.</summary>
    public RatingCurve Curve { get; set; } = new();
}
