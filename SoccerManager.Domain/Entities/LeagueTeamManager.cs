namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a manager's appointment to a team within a league, from the date the tenure started until the date it ended.
/// </summary>
public class LeagueTeamManager : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the league the appointment belongs to.
    /// </summary>
    public int LeagueId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the team the manager is appointed to.
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the manager the appointment belongs to.
    /// </summary>
    public int ManagerId { get; set; }

    /// <summary>
    /// Gets or sets the date the tenure started.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// Gets or sets the date the tenure ended, or <see langword="null"/> while it is still current.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Gets or sets whether the appointment is the team's current manager in the league.
    /// </summary>
    public bool IsCurrent { get; set; }
}
