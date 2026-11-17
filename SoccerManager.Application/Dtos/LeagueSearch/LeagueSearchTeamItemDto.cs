namespace SoccerManager.Application.Dtos.LeagueSearch;

/// <summary>
/// Represents a club found by a league search.
/// </summary>
public sealed record LeagueSearchTeamItemDto
{
    /// <summary>
    /// The identifier of the club's entry on the league's roster.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The name of the team.
    /// </summary>
    public required string TeamName { get; init; }

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; init; }
}
