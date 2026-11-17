namespace SoccerManager.Application.Dtos.LeagueSearch;

/// <summary>
/// Represents a player found by a league search, with the club the player is registered at in the league, the player's
/// current rating and main position.
/// </summary>
public sealed record LeagueSearchPlayerItemDto
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
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; init; }

    /// <summary>
    /// The name of the player's main position, empty when the player has none.
    /// </summary>
    public required string PositionName { get; init; }

    /// <summary>
    /// The short name of the player's main position, for example CB, empty when the player has none.
    /// </summary>
    public required string PositionShortName { get; init; }

    /// <summary>
    /// The identifier of the league team the player is registered at in the league (the searching club's own registration
    /// first, else the lowest registration), or <see langword="null"/> when the player is registered at no club of it: a
    /// free agent.
    /// </summary>
    public int? LeagueTeamId { get; init; }

    /// <summary>
    /// The name of that club, or <see langword="null"/> for a free agent.
    /// </summary>
    public string? TeamName { get; init; }
}
