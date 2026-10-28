namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a player on a league team's squad, with their current rating and main position.
/// </summary>
public sealed record LeagueTeamSquadPlayerDto
{
    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the player.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; init; }

    /// <summary>
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The identifier of the player's main (highest-quality) position, or <see langword="null"/> when it has none.
    /// </summary>
    public int? PositionId { get; init; }

    /// <summary>
    /// The name of the player's main position, or <see langword="null"/> when it has none.
    /// </summary>
    public string? PositionName { get; init; }
}
