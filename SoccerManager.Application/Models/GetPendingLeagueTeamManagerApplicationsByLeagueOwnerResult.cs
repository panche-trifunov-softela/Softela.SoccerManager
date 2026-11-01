namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetPendingLeagueTeamManagerApplicationsByLeagueOwner</c> stored procedure: a
/// pending application, denormalised with the league and team names and the applicant's name and photo the procedure
/// joins in. Kept separate from <see cref="Dtos.IncomingLeagueTeamManagerApplicationDto"/> because Dapper populates a
/// result by setting properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetPendingLeagueTeamManagerApplicationsByLeagueOwnerResult
{
    /// <summary>
    /// The identifier of the application.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The identifier of the league roster entry the application is for.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// The identifier of the league the application was made in, joined in through its roster entry.
    /// </summary>
    public int LeagueId { get; set; }

    /// <summary>
    /// The name of the league the application was made in.
    /// </summary>
    public string LeagueName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the team the application is for.
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; set; }

    /// <summary>
    /// The identifier of the manager who applied.
    /// </summary>
    public int ManagerId { get; set; }

    /// <summary>
    /// The applicant's display name (first and last name), or <see langword="null"/> when none is known.
    /// </summary>
    public string? ManagerName { get; set; }

    /// <summary>
    /// The URL of the applicant's photo, or <see langword="null"/> when they have none.
    /// </summary>
    public string? ManagerImageUrl { get; set; }

    /// <summary>
    /// The UTC date and time the application was made.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
