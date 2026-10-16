namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a player rating for read-oriented consumers.
/// </summary>
public sealed record PlayerRatingDto
{
    /// <summary>
    /// The identifier of the player rating.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the player the rating belongs to.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The rating value, from 1 to 100.
    /// </summary>
    public int Rating { get; init; }

    /// <summary>
    /// Whether this is the player's rating in force.
    /// </summary>
    public bool IsCurrent { get; init; }

    /// <summary>
    /// The UTC date and time the rating was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the rating was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
