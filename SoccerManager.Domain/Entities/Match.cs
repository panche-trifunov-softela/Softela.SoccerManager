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
    /// Gets or sets the identifier of the division the match is played in.
    /// </summary>
    public int DivisionId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the match's referee.
    /// There is deliberately no foreign key here: <c>dbo.Referees</c> does not exist yet, so a constraint
    /// referencing it would make the Evolve migration fail at startup.
    /// </summary>
    public int RefereeId { get; set; }

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
