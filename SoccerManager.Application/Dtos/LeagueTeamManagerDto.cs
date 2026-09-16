namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a league team manager appointment for read-oriented consumers.
/// </summary>
public sealed record LeagueTeamManagerDto
{
    /// <summary>
    /// The identifier of the league team manager appointment.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the league the appointment belongs to.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The identifier of the team the manager is appointed to.
    /// </summary>
    public int TeamId { get; init; }

    /// <summary>
    /// The identifier of the manager the appointment belongs to.
    /// </summary>
    public int ManagerId { get; init; }

    /// <summary>
    /// The date the tenure started.
    /// </summary>
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// The date the tenure ended, or <see langword="null"/> while it is still current.
    /// </summary>
    public DateOnly? EndDate { get; init; }

    /// <summary>
    /// Whether the appointment is the team's current manager in the league.
    /// </summary>
    public bool IsCurrent { get; init; }

    /// <summary>
    /// The UTC date and time the appointment was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the appointment was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
