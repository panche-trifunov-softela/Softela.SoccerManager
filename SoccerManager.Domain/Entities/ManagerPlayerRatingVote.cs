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
    /// Gets or sets the identifier of the player rating the vote was cast on; the player is reached through it.
    /// </summary>
    public int PlayerRatingId { get; set; }

    /// <summary>
    /// Gets or sets whether the vote is up (<see langword="true"/>) or down (<see langword="false"/>).
    /// </summary>
    public bool UpOrDown { get; set; }
}
