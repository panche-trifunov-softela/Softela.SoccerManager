namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a player position rating for read-oriented consumers.
/// </summary>
public sealed record PlayerPositionDto
{
    /// <summary>
    /// The identifier of the player position rating.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the player the rating belongs to.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The identifier of the position being rated.
    /// </summary>
    public int PositionId { get; init; }

    /// <summary>
    /// How well the player performs in that position, from 1 to 100.
    /// </summary>
    public int Quality { get; init; }

    /// <summary>
    /// The UTC date and time the rating was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the rating was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
