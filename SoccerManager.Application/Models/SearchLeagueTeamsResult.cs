namespace SoccerManager.Application.Models;

/// <summary>
/// One club row of the fourth result set returned by the <c>dbo.SearchLeaguePlayersAndTeams</c> stored procedure: a club of
/// the league whose team name matches the search. Kept separate from
/// <see cref="Dtos.LeagueSearch.LeagueSearchTeamItemDto"/> because Dapper populates a result by setting properties, and the
/// Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class SearchLeagueTeamsResult
{
    /// <summary>
    /// The identifier of the club's entry on the league's roster.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// The name of the team.
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; set; }
}
