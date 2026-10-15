namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a manager's vote on whether a player's rating should go up or down.
/// </summary>
public class ManagerPlayerRatingVote : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the manager who cast the vote.
    /// </summary>
    public int ManagerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player the vote is about.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets the player's rating when the vote was cast.
    /// </summary>
    public int RatingAtTheMoment { get; set; }

    /// <summary>
    /// Gets or sets whether the vote is up (<see langword="true"/>) or down (<see langword="false"/>).
    /// </summary>
    public bool UpOrDown { get; set; }
}
