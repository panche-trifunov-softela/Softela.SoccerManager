namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one rating a player has had; the current one is the rating in force.
/// </summary>
public class PlayerRating : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the player the rating belongs to.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets the rating value, from 1 to 100.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Gets or sets whether this is the player's rating in force; a player has exactly one current rating.
    /// </summary>
    public bool IsCurrent { get; set; }
}
