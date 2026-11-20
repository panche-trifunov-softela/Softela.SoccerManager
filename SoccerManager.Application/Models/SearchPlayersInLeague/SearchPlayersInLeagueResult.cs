namespace SoccerManager.Application.Models.SearchPlayersInLeague;

/// <summary>
/// One row returned by the <c>dbo.SearchPlayersInLeague</c> stored procedure: a player denormalised with the club he is
/// registered at in the league and the names of his national team and main position. Kept separate from
/// <see cref="Dtos.PlayersInLeague.PlayersInLeagueItemDto"/> because Dapper populates a result by setting properties, and
/// the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class SearchPlayersInLeagueResult
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
    /// The player's date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// The player's value: the in-game one for a registered player, the real-world one for a free agent.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// The player's wage: the in-game one for a registered player, the real-world one for a free agent.
    /// </summary>
    public decimal Wage { get; set; }

    /// <summary>
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// The identifier of the league team the player is registered at in the league, or <see langword="null"/> for a free
    /// agent.
    /// </summary>
    public int? LeagueTeamId { get; set; }

    /// <summary>
    /// The name of that club, or <see langword="null"/> for a free agent.
    /// </summary>
    public string? TeamName { get; set; }

    /// <summary>
    /// The identifier of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public int? NationalTeamId { get; set; }

    /// <summary>
    /// The name of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public string? NationalTeamName { get; set; }

    /// <summary>
    /// The identifier of the player's main (highest-quality) position, 0 when the player has none.
    /// </summary>
    public int PositionId { get; set; }

    /// <summary>
    /// The name of the player's main position, empty when the player has none.
    /// </summary>
    public string PositionName { get; set; } = string.Empty;

    /// <summary>
    /// The short name of the player's main position, for example CB, empty when the player has none.
    /// </summary>
    public string PositionShortName { get; set; } = string.Empty;
}
