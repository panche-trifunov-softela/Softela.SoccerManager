namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetLeagueTeamSquad</c> stored procedure: a player on a league team's squad, denormalised
/// with the player's current rating and the name of their main position. Kept separate from
/// <see cref="Dtos.LeagueTeamSquadPlayerDto"/> because Dapper populates a result by setting properties, and the Dto is an
/// init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeagueTeamSquadResult
{
    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the player.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// The identifier of the player's main (highest-quality) position, or <see langword="null"/> when it has none.
    /// </summary>
    public int? PositionId { get; set; }

    /// <summary>
    /// The name of the player's main position, or <see langword="null"/> when it has none.
    /// </summary>
    public string? PositionName { get; set; }

    /// <summary>
    /// The short name of the player's main position, for example CB, or <see langword="null"/> when they have none.
    /// </summary>
    public string? PositionShortName { get; set; }
}
