namespace SoccerManager.Application.Dtos.PlayersInLeague;

/// <summary>
/// Represents a player as the league player search lists it, with the club he is registered at in the league and the names
/// of his national team and main position.
/// </summary>
public sealed record PlayersInLeagueItemDto
{
    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The name of the player.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The player's date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; init; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; init; }

    /// <summary>
    /// The player's value: the in-game one for a registered player, the real-world one for a free agent.
    /// </summary>
    public decimal Value { get; init; }

    /// <summary>
    /// The player's wage: the in-game one for a registered player, the real-world one for a free agent.
    /// </summary>
    public decimal Wage { get; init; }

    /// <summary>
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The identifier of the league team the player is registered at in the league, or <see langword="null"/> for a free
    /// agent.
    /// </summary>
    public int? LeagueTeamId { get; init; }

    /// <summary>
    /// The name of that club, or <see langword="null"/> for a free agent.
    /// </summary>
    public string? TeamName { get; init; }

    /// <summary>
    /// The identifier of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public int? NationalTeamId { get; init; }

    /// <summary>
    /// The name of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public string? NationalTeamName { get; init; }

    /// <summary>
    /// The identifier of the player's main (highest-quality) position, 0 when the player has none.
    /// </summary>
    public int PositionId { get; init; }

    /// <summary>
    /// The name of the player's main position, empty when the player has none.
    /// </summary>
    public required string PositionName { get; init; }

    /// <summary>
    /// The short name of the player's main position, for example CB, empty when the player has none.
    /// </summary>
    public required string PositionShortName { get; init; }
}
