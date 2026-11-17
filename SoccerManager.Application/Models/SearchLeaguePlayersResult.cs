namespace SoccerManager.Application.Models;

/// <summary>
/// One player row of the second result set returned by the <c>dbo.SearchLeaguePlayersAndTeams</c> stored procedure: a player
/// whose name matches the search, with the club the player is registered at in the league and the player's current rating
/// and main position. Kept separate from <see cref="Dtos.LeagueSearch.LeagueSearchPlayerItemDto"/> because Dapper populates
/// a result by setting properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class SearchLeaguePlayersResult
{
    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// The name of the player.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// The name of the player's main position, empty when the player has none.
    /// </summary>
    public string PositionName { get; set; } = string.Empty;

    /// <summary>
    /// The short name of the player's main position, for example CB, empty when the player has none.
    /// </summary>
    public string PositionShortName { get; set; } = string.Empty;

    /// <summary>
    /// The identifier of the league team the player is registered at in the league, or <see langword="null"/> when the
    /// player is registered at no club of it: a free agent.
    /// </summary>
    public int? LeagueTeamId { get; set; }

    /// <summary>
    /// The name of that club, or <see langword="null"/> for a free agent.
    /// </summary>
    public string? TeamName { get; set; }
}
