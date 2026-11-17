namespace SoccerManager.Application.Models;

/// <summary>
/// The row returned by the <c>dbo.GetLeagueTeamProfile</c> stored procedure: a club's entry on a league's roster,
/// denormalised with the league and team names the procedure joins in. Kept separate from
/// <see cref="Dtos.LeagueTeamProfileDto"/> because Dapper populates a result by setting properties, and the Dto is an
/// init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeagueTeamProfileResult
{
    /// <summary>
    /// The identifier of the club's entry on the league's roster.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// The identifier of the league the club plays in.
    /// </summary>
    public int LeagueId { get; set; }

    /// <summary>
    /// The name of the league the club plays in.
    /// </summary>
    public string LeagueName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the team.
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; set; }
}
