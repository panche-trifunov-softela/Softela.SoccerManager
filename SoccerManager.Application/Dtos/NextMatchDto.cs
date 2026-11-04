namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a club's next match, with its opponent and the club's lineup and instructions for it.
/// </summary>
public sealed record NextMatchDto
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
    /// The identifier of the opponent's league team, or <see langword="null"/> when the match has no second participant.
    /// </summary>
    public int? OpponentLeagueTeamId { get; init; }

    /// <summary>
    /// The name of the opponent's team, or <see langword="null"/> when the match has no second participant.
    /// </summary>
    public string? OpponentTeamName { get; init; }

    /// <summary>
    /// The URL of the opponent's logo, or <see langword="null"/> when there is no opponent or no logo.
    /// </summary>
    public string? OpponentTeamLogoUrl { get; init; }

    /// <summary>
    /// The club's lineup for the match.
    /// </summary>
    public required MatchLineupDto Lineup { get; init; }

    /// <summary>
    /// The club's instructions for the match: the ones saved for it, else the ones it last saved for an earlier match, else
    /// the defaults.
    /// </summary>
    public required MatchInstructionsDto Instructions { get; init; }
}
