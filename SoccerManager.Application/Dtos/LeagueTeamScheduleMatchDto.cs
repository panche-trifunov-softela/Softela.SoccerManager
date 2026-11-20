using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a match on a club's schedule: when it is played, against whom and where, in which competition and round, and
/// its result once it has finished.
/// </summary>
public sealed record LeagueTeamScheduleMatchDto
{
    /// <summary>
    /// The identifier of the match.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The UTC date and time the match is due to start.
    /// </summary>
    public DateTime StartDateTime { get; init; }

    /// <summary>
    /// Whether the club plays the match at home.
    /// </summary>
    public bool IsHomeTeam { get; init; }

    /// <summary>
    /// The name of the match's competition, or <see langword="null"/> for a friendly.
    /// </summary>
    public string? CompetitionName { get; init; }

    /// <summary>
    /// The round of the match in words, for example Round 2 or Quarter-final, 1st leg, or <see langword="null"/> when the
    /// match has no round to name.
    /// </summary>
    public string? StageName { get; init; }

    /// <summary>
    /// The name of the opponent's team, or <see langword="null"/> when the opponent is not decided or has no name.
    /// </summary>
    public string? OpponentTeamName { get; init; }

    /// <summary>
    /// The URL of the opponent's logo, or <see langword="null"/> when there is no opponent or no logo.
    /// </summary>
    public string? OpponentTeamLogoUrl { get; init; }

    /// <summary>
    /// The goals scored for the club, or <see langword="null"/> until the match is finished.
    /// </summary>
    public int? GoalsFor { get; init; }

    /// <summary>
    /// The goals scored against the club, or <see langword="null"/> until the match is finished.
    /// </summary>
    public int? GoalsAgainst { get; init; }

    /// <summary>
    /// The club's scored shoot-out kicks, or <see langword="null"/> unless the match finished with a shoot-out.
    /// </summary>
    public int? ShootoutGoalsFor { get; init; }

    /// <summary>
    /// The opponent's scored shoot-out kicks, or <see langword="null"/> unless the match finished with a shoot-out.
    /// </summary>
    public int? ShootoutGoalsAgainst { get; init; }

    /// <summary>
    /// How the match ended for the club, or <see langword="null"/> until the match is finished.
    /// </summary>
    public MatchOutcome? Outcome { get; init; }

    /// <summary>
    /// How the match ended for the club in words, for example Win, or <see langword="null"/> until the match is finished.
    /// </summary>
    public string? OutcomeLabel { get; init; }
}
