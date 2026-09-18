namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one team's statistics for one match.
/// </summary>
public class MatchTeamStatistic : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the team the statistics belong to.
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the match the statistics belong to.
    /// </summary>
    public int MatchId { get; set; }

    /// <summary>
    /// Gets or sets whether the team played the match at home.
    /// </summary>
    public bool IsHomeTeam { get; set; }

    /// <summary>
    /// Gets or sets the number of shots taken, on target or not.
    /// </summary>
    public int ShotsTotal { get; set; }

    /// <summary>
    /// Gets or sets the number of shots on target. These are a subset of
    /// <see cref="ShotsTotal"/>, so this value is never greater than it.
    /// </summary>
    public int ShotsOnTarget { get; set; }

    /// <summary>
    /// Gets or sets the share of possession as a whole-number percentage, from 0 to 100 inclusive.
    /// </summary>
    public int Possession { get; set; }
}
