namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the column a player search is ordered by. The values start at 1 like every enum here, and
/// they reach the database as numbers, so they must not be renumbered.
/// </summary>
public enum PlayerSortBy : byte
{
    /// <summary>
    /// Orders by the player's name.
    /// </summary>
    Name = 1,

    /// <summary>
    /// Orders by the player's overall rating.
    /// </summary>
    Rating = 2,

    /// <summary>
    /// Orders by the player's age, so ascending puts the youngest first.
    /// </summary>
    Age = 3,

    /// <summary>
    /// Orders by the player's market value.
    /// </summary>
    Value = 4,

    /// <summary>
    /// Orders by the player's wage.
    /// </summary>
    Wage = 5,
}
