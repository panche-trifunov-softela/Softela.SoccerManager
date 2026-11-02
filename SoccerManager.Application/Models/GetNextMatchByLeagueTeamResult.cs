namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetNextMatchByLeagueTeam</c> stored procedure: a club's earliest match that has not
/// started, with whether the club plays at home and who the opponent is. Kept separate from
/// <see cref="Dtos.NextMatchDto"/> because Dapper populates a result by setting properties, and the Dto is an init-only
/// record that cannot be a Dapper target.
/// </summary>
public class GetNextMatchByLeagueTeamResult
{
    /// <summary>
    /// The identifier of the match.
    /// </summary>
    public int MatchId { get; set; }

    /// <summary>
    /// The UTC date and time the match is due to start.
    /// </summary>
    public DateTime StartDateTime { get; set; }

    /// <summary>
    /// Whether the club plays the match at home.
    /// </summary>
    public bool IsHomeTeam { get; set; }

    /// <summary>
    /// The identifier of the opponent's league team, or <see langword="null"/> when the match has no second participant.
    /// </summary>
    public int? OpponentLeagueTeamId { get; set; }

    /// <summary>
    /// The name of the opponent's team, or <see langword="null"/> when the match has no second participant.
    /// </summary>
    public string? OpponentTeamName { get; set; }

    /// <summary>
    /// The URL of the opponent's logo, or <see langword="null"/> when there is no opponent or no logo.
    /// </summary>
    public string? OpponentTeamLogoUrl { get; set; }
}
