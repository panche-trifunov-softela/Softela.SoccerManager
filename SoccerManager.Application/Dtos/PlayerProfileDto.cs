namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a player's profile from the player catalogue: the player with the real-life club, national team, main
/// position, current rating, value and wage, and every position the player can play.
/// </summary>
public sealed record PlayerProfileDto
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
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The player's date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; init; }

    /// <summary>
    /// The identifier of the player's current real-life club, or <see langword="null"/> when it has none.
    /// </summary>
    public int? TeamId { get; init; }

    /// <summary>
    /// The name of the player's current real-life club, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamName { get; init; }

    /// <summary>
    /// The URL of the club's logo, or <see langword="null"/> when there is no club or no logo.
    /// </summary>
    public string? TeamLogoUrl { get; init; }

    /// <summary>
    /// The identifier of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public int? NationalTeamId { get; init; }

    /// <summary>
    /// The name of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public string? NationalTeamName { get; init; }

    /// <summary>
    /// The URL of the national team's logo, or <see langword="null"/> when there is no national team or no logo.
    /// </summary>
    public string? NationalTeamLogoUrl { get; init; }

    /// <summary>
    /// The identifier of the player's main (highest-quality) position.
    /// </summary>
    public int PositionId { get; init; }

    /// <summary>
    /// The name of the player's main position.
    /// </summary>
    public required string PositionName { get; init; }

    /// <summary>
    /// The short name of the player's main position, for example CB.
    /// </summary>
    public required string PositionShortName { get; init; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; init; }

    /// <summary>
    /// The player's market value.
    /// </summary>
    public decimal Value { get; init; }

    /// <summary>
    /// The player's wage.
    /// </summary>
    public decimal Wage { get; init; }

    /// <summary>
    /// Every position the player can play, the main position first.
    /// </summary>
    public required IReadOnlyList<PlayerProfilePositionDto> Positions { get; init; }
}
