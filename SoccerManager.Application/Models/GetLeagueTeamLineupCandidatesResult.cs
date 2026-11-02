using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetLeagueTeamLineupCandidates</c> stored procedure: a registered squad player paired with
/// one of their position ratings, for the caller to group by player. A player with no position rating is a single row
/// with the position columns <see langword="null"/>. Dapper populates a result by setting properties, so the row is its
/// own class rather than a domain entity.
/// </summary>
public class GetLeagueTeamLineupCandidatesResult
{
    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// The player's squad number, from 1 to 99 inclusive.
    /// </summary>
    public int SquadNumber { get; set; }

    /// <summary>
    /// The player's physical condition, from 1 to 100 inclusive.
    /// </summary>
    public int Condition { get; set; }

    /// <summary>
    /// Whether the player is suspended in the domestic competition.
    /// </summary>
    public bool IsSuspended { get; set; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// The identifier of the player position rating, or <see langword="null"/> when the player has none.
    /// </summary>
    public int? PlayerPositionId { get; set; }

    /// <summary>
    /// The identifier of the position the rating is for, or <see langword="null"/> when the player has none.
    /// </summary>
    public int? PositionId { get; set; }

    /// <summary>
    /// The quality of the player at the position, or <see langword="null"/> when the player has no rating.
    /// </summary>
    public int? Quality { get; set; }

    /// <summary>
    /// The area of the pitch the position occupies, or <see langword="null"/> when the player has no rating.
    /// </summary>
    public PositionArea? PositionArea { get; set; }
}
