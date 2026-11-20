namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetScheduleByLeagueTeam</c> stored procedure: a match of a club in its League's latest
/// season, with whether the club plays at home, the competition and round, the opponent and each side's score. Kept
/// separate from <see cref="Dtos.LeagueTeamScheduleMatchDto"/> because Dapper populates a result by setting properties, and
/// the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetScheduleByLeagueTeamResult
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
    /// Whether the match has finished, which is when its goals are the result.
    /// </summary>
    public bool IsFinished { get; set; }

    /// <summary>
    /// The name of the match's competition, or <see langword="null"/> for a friendly.
    /// </summary>
    public string? CompetitionName { get; set; }

    /// <summary>
    /// The stage of the competition the match is in, as a <see cref="Domain.Enums.MatchStage"/> value, or
    /// <see langword="null"/> when it has none.
    /// </summary>
    public byte? Stage { get; set; }

    /// <summary>
    /// The round of the stage, or <see langword="null"/> when the match has none.
    /// </summary>
    public byte? RoundNumber { get; set; }

    /// <summary>
    /// The leg of a two-legged tie, 1 or 2, or <see langword="null"/> for a single match.
    /// </summary>
    public byte? Leg { get; set; }

    /// <summary>
    /// The name of the opponent's team, or <see langword="null"/> when the match has no second participant or the opponent
    /// has no name.
    /// </summary>
    public string? OpponentTeamName { get; set; }

    /// <summary>
    /// The URL of the opponent's logo, or <see langword="null"/> when there is no opponent or no logo.
    /// </summary>
    public string? OpponentTeamLogoUrl { get; set; }

    /// <summary>
    /// The goals scored for the club, own goals by the opponent included; meaningful only when the match is finished.
    /// </summary>
    public int GoalsFor { get; set; }

    /// <summary>
    /// The goals scored against the club, own goals by the club included; meaningful only when the match is finished.
    /// </summary>
    public int GoalsAgainst { get; set; }

    /// <summary>
    /// The club's scored shoot-out kicks, or <see langword="null"/> when the match had no shoot-out.
    /// </summary>
    public int? ShootoutGoalsFor { get; set; }

    /// <summary>
    /// The opponent's scored shoot-out kicks, or <see langword="null"/> when the match had no shoot-out.
    /// </summary>
    public int? ShootoutGoalsAgainst { get; set; }
}
