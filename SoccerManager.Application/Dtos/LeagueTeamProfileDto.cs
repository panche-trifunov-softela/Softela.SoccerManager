namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a club's identity: its entry on a league's roster, the league it plays in, and the team's name and
/// logo, the same for every caller.
/// </summary>
public sealed record LeagueTeamProfileDto
{
    /// <summary>
    /// The identifier of the club's entry on the league's roster.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the league the club plays in.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The name of the league the club plays in.
    /// </summary>
    public required string LeagueName { get; init; }

    /// <summary>
    /// The name of the team.
    /// </summary>
    public required string TeamName { get; init; }

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; init; }
}
