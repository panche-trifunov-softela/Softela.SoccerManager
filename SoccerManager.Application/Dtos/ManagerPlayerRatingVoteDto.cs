namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a manager's vote on a player's rating for read-oriented consumers.
/// </summary>
public sealed record ManagerPlayerRatingVoteDto
{
    /// <summary>
    /// The identifier of the vote.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the manager who cast the vote.
    /// </summary>
    public int ManagerId { get; init; }

    /// <summary>
    /// The identifier of the player the vote is about.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The player's rating when the vote was cast.
    /// </summary>
    public int RatingAtTheMoment { get; init; }

    /// <summary>
    /// Whether the vote is up; false means down.
    /// </summary>
    public bool UpOrDown { get; init; }

    /// <summary>
    /// The UTC date and time the vote was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the vote was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
