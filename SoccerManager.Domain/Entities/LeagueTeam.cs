namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a team's place on the roster of a league (gameworld), without any player data.
/// </summary>
public class LeagueTeam : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the league the team takes part in.
    /// </summary>
    public int LeagueId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the team taking part in the league.
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the team is a guest of the league: on its roster so its players can be
    /// bought, but never managed and never in a division.
    /// </summary>
    public bool IsGuest { get; set; }
}
