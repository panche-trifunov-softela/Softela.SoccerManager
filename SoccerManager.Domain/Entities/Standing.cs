namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one team's record in a competition for a season.
/// </summary>
public class Standing : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the competition this record belongs to.
    /// </summary>
    public int CompetitionId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the season this record belongs to.
    /// </summary>
    public int SeasonId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the team this record is for.
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// Gets or sets the points accumulated.
    /// </summary>
    public int Points { get; set; }

    /// <summary>
    /// Gets or sets the goals scored.
    /// </summary>
    public int GoalsFor { get; set; }

    /// <summary>
    /// Gets or sets the goals conceded.
    /// </summary>
    public int GoalsAgainst { get; set; }

    /// <summary>
    /// Gets or sets the number of matches won.
    /// </summary>
    public int Wins { get; set; }

    /// <summary>
    /// Gets or sets the number of matches drawn.
    /// </summary>
    public int Draws { get; set; }

    /// <summary>
    /// Gets or sets the number of matches lost.
    /// </summary>
    public int Losses { get; set; }
}
