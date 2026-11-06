namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how well a player suits a position, worked out from their quality there.
/// </summary>
public enum PositionFit : byte
{
    /// <summary>
    /// The position is the player's main one.
    /// </summary>
    Main = 1,

    /// <summary>
    /// The player is a natural in the position, though it is not their main one.
    /// </summary>
    NaturalFit = 2,

    /// <summary>
    /// The player can play the position. An unrated position in the same area as their main one reads as this.
    /// </summary>
    CanPlay = 3,

    /// <summary>
    /// The player is a poor fit for the position. An unrated position in another area than their main one reads as this.
    /// </summary>
    PoorFit = 4,
}
