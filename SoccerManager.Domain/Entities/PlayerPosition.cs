namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents how well a player performs in a given position.
/// </summary>
public class PlayerPosition : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the player the rating belongs to.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the position being rated.
    /// </summary>
    public int PositionId { get; set; }

    /// <summary>
    /// Gets or sets how well the player performs in that position, from 1 to 100.
    /// </summary>
    public int Quality { get; set; }
}
