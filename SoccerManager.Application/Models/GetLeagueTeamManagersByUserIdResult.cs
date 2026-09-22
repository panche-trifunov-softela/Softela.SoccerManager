namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetLeagueTeamManagersByUserId</c> stored procedure: a manager's
/// appointment, denormalised with the league and team names the procedure joins in. Kept separate
/// from <see cref="Dtos.MyLeagueTeamManagerDto"/> because Dapper populates a result by setting
/// properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeagueTeamManagersByUserIdResult
{
    /// <summary>
    /// The identifier of the league team manager appointment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The identifier of the league the appointment belongs to.
    /// </summary>
    public int LeagueId { get; set; }

    /// <summary>
    /// The name of the league the appointment belongs to.
    /// </summary>
    public string LeagueName { get; set; } = string.Empty;

    /// <summary>
    /// The identifier of the team the manager is appointed to.
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// The name of the team the manager is appointed to.
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; set; }

    /// <summary>
    /// The identifier of the manager the appointment belongs to.
    /// </summary>
    public int ManagerId { get; set; }

    /// <summary>
    /// The date the tenure started.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// The date the tenure ended, or <see langword="null"/> while it is still current.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Whether the appointment is the team's current manager in the league.
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// The UTC date and time the appointment was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The UTC date and time the appointment was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
