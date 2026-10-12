namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a team's place on the roster of a league for read-oriented consumers.
/// </summary>
public sealed record LeagueTeamDto
{
    /// <summary>
    /// The identifier of the league team.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the league the team takes part in.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The identifier of the team taking part in the league.
    /// </summary>
    public int TeamId { get; init; }

    /// <summary>
    /// The UTC date and time the league team was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the league team was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
