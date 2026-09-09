namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a tier within a league.
/// </summary>
public class Division : BaseEntity
{
    /// <summary>
    /// Gets or sets the division's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the division's rank within its league, where lower values indicate higher tiers.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the league this division belongs to.
    /// </summary>
    public int LeagueId { get; set; }

    /// <summary>
    /// Gets or sets the number of teams promoted from this division at the end of a season.
    /// </summary>
    public int TeamsPromoted { get; set; }

    /// <summary>
    /// Gets or sets the number of teams relegated from this division at the end of a season.
    /// </summary>
    public int TeamsRelegated { get; set; }

    /// <summary>
    /// Gets or sets the number of teams from this division that enter the playoffs.
    /// </summary>
    public int TeamsInPlayoffs { get; set; }
}
