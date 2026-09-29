namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a scheduled match between two teams.
/// </summary>
public class Match : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the season the match is played in.
    /// </summary>
    public int SeasonId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the competition the match is played in, or <see langword="null"/> when it is a friendly.
    /// </summary>
    public int? CompetitionId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the match's referee, or <see langword="null"/> when it has none.
    /// </summary>
    public int? RefereeId { get; set; }

    /// <summary>
    /// Gets or sets the UTC moment the match starts.
    /// </summary>
    public DateTime StartDateTime { get; set; }

    /// <summary>
    /// Gets or sets the match commentary as a JSON document, or <see langword="null"/> when there is none.
    /// </summary>
    public string? Commentary { get; set; }

    /// <summary>
    /// Gets or sets the number of spectators at the match; zero until it has been played.
    /// </summary>
    public int Attendance { get; set; }

    /// <summary>
    /// Gets or sets whether the match has started.
    /// </summary>
    public bool IsStarted { get; set; }
}
