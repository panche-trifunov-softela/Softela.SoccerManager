namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one player's statistics for one match.
/// </summary>
public class MatchPlayerStatistic : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the player the statistics belong to.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the match the statistics belong to.
    /// </summary>
    public int MatchId { get; set; }

    /// <summary>
    /// Gets or sets the player's rating for the match, from 5.0 to 10.0 inclusive.
    /// </summary>
    public decimal Rating { get; set; }

    /// <summary>
    /// Gets or sets whether the player started the match.
    /// </summary>
    public bool IsStarter { get; set; }

    /// <summary>
    /// Gets or sets the number of minutes the player played.
    /// </summary>
    public int MinutesPlayed { get; set; }

    /// <summary>
    /// Gets or sets the total number of goals scored, including penalties scored.
    /// </summary>
    public int Goals { get; set; }

    /// <summary>
    /// Gets or sets the number of assists made.
    /// </summary>
    public int Assists { get; set; }

    /// <summary>
    /// Gets or sets the number of penalties scored. Each one is also counted in
    /// <see cref="Goals"/>, so <see cref="Goals"/> is never lower than this value.
    /// </summary>
    public int PenaltiesScored { get; set; }

    /// <summary>
    /// Gets or sets the number of penalties missed.
    /// </summary>
    public int PenaltiesMissed { get; set; }

    /// <summary>
    /// Gets or sets the number of yellow cards received.
    /// </summary>
    public int YellowCards { get; set; }

    /// <summary>
    /// Gets or sets the number of red cards received.
    /// </summary>
    public int RedCards { get; set; }
}
