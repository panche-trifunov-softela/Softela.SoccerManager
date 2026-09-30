namespace SoccerManager.Importer.Transform;

/// <summary>
/// Configuration for deriving a player's secondary positions from their starting lineup history.
/// </summary>
public sealed class PositionsOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Transform:Positions";

    /// <summary>The minimum number of starts at a position, other than the main one, for it to count as secondary.</summary>
    public int SecondaryMinStarts { get; set; } = 3;

    /// <summary>The minimum share of the player's total starts a position must reach, other than the main one, to count as secondary.</summary>
    public double SecondaryMinShare { get; set; } = 0.10;

    /// <summary>The quality floor a secondary position's score is built up from.</summary>
    public double SecondaryBase { get; set; } = 60;

    /// <summary>The span added to <see cref="SecondaryBase"/>, scaled by the position's share of the player's busiest position.</summary>
    public double SecondarySpan { get; set; } = 35;

    /// <summary>The maximum quality a secondary position can be assigned.</summary>
    public double SecondaryCap { get; set; } = 95;
}
