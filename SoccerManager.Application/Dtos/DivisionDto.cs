namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a division for read-oriented consumers.
/// </summary>
public sealed record DivisionDto
{
    /// <summary>
    /// The identifier of the division.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the division.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The division's rank within its league, where lower values indicate higher tiers.
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// The identifier of the league the division belongs to.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The number of teams promoted from this division at the end of a season.
    /// </summary>
    public int TeamsPromoted { get; init; }

    /// <summary>
    /// The number of teams relegated from this division at the end of a season.
    /// </summary>
    public int TeamsRelegated { get; init; }

    /// <summary>
    /// The number of teams from this division that enter the playoffs.
    /// </summary>
    public int TeamsInPlayoffs { get; init; }

    /// <summary>
    /// The UTC date and time the division was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the division was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
