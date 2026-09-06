namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a single season played within a league.
/// </summary>
public class Season : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the league this season belongs to.
    /// </summary>
    public int LeagueId { get; set; }

    /// <summary>
    /// Gets or sets the sequential number of this season within its league.
    /// </summary>
    public int SeasonNumber { get; set; }
}
