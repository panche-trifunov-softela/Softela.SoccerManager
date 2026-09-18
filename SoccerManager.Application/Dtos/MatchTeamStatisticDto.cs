namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a match team statistic for read-oriented consumers.
/// </summary>
public sealed record MatchTeamStatisticDto
{
    /// <summary>
    /// The identifier of the match team statistic.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the team the statistics belong to.
    /// </summary>
    public int TeamId { get; init; }

    /// <summary>
    /// The identifier of the match the statistics belong to.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// Whether the team played the match at home.
    /// </summary>
    public bool IsHomeTeam { get; init; }

    /// <summary>
    /// The number of shots taken, on target or not.
    /// </summary>
    public int ShotsTotal { get; init; }

    /// <summary>
    /// The number of shots on target. These are a subset of <see cref="ShotsTotal"/>,
    /// so this value is never greater than it.
    /// </summary>
    public int ShotsOnTarget { get; init; }

    /// <summary>
    /// The share of possession as a whole-number percentage, from 0 to 100 inclusive.
    /// </summary>
    public int Possession { get; init; }

    /// <summary>
    /// The UTC date and time the match team statistic was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the match team statistic was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
