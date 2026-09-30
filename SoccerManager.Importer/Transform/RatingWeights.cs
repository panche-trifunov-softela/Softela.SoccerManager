namespace SoccerManager.Importer.Transform;

/// <summary>
/// The rating component weights for a single position group. The seven weights are expected to sum to 1.00.
/// </summary>
public sealed class RatingWeights
{
    /// <summary>The weight of the market value component.</summary>
    public double MarketValue { get; set; }

    /// <summary>The weight of the minutes-share component.</summary>
    public double Minutes { get; set; }

    /// <summary>The weight of the goal-involvement output component.</summary>
    public double Output { get; set; }

    /// <summary>The weight of the club's defensive record component.</summary>
    public double Defence { get; set; }

    /// <summary>The weight of the club's league record component.</summary>
    public double Team { get; set; }

    /// <summary>The weight of the club's league strength component.</summary>
    public double League { get; set; }

    /// <summary>The weight of the international caps component.</summary>
    public double Caps { get; set; }
}
