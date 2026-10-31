namespace SoccerManager.Application.Dtos;

/// <summary>
/// A pending application to manage a club in a league the authenticated caller owns (in any league, for an operator), with
/// the league and team names and the applicant's photo for display.
/// </summary>
public sealed record IncomingLeagueTeamManagerApplicationDto
{
    /// <summary>
    /// The identifier of the application.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the league roster entry (the club in its league) the application is for.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the league the application was made in.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The name of the league the application was made in.
    /// </summary>
    public required string LeagueName { get; init; }

    /// <summary>
    /// The name of the team the application is for.
    /// </summary>
    public required string TeamName { get; init; }

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; init; }

    /// <summary>
    /// The identifier of the manager who applied.
    /// </summary>
    public int ManagerId { get; init; }

    /// <summary>
    /// The URL of the applicant's photo, or <see langword="null"/> when they have none.
    /// </summary>
    public string? ManagerImageUrl { get; init; }

    /// <summary>
    /// When the application was made, in UTC.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
