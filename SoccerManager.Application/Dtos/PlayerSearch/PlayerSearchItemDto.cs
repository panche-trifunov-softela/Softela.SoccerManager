namespace SoccerManager.Application.Dtos.PlayerSearch;

/// <summary>
/// Represents a player as a players search lists it, with the names of its club, national team and main position, and the
/// caller's vote on its current rating.
/// </summary>
public sealed record PlayerSearchItemDto
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
    /// The player's date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; init; }

    /// <summary>
    /// The player's overall rating.
    /// </summary>
    public int Rating { get; init; }

    /// <summary>
    /// The player's market value.
    /// </summary>
    public decimal Value { get; init; }

    /// <summary>
    /// The player's wage.
    /// </summary>
    public decimal Wage { get; init; }

    /// <summary>
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The identifier of the player's current real-life club, or <see langword="null"/> when it has none.
    /// </summary>
    public int? TeamId { get; init; }

    /// <summary>
    /// The name of the player's current real-life club, or <see langword="null"/> when it has none.
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
    /// The identifier of the player's main (highest-quality) position, or <see langword="null"/> when it has none.
    /// </summary>
    public int? PositionId { get; init; }

    /// <summary>
    /// The name of the player's main position, or <see langword="null"/> when it has none.
    /// </summary>
    public string? PositionName { get; init; }

    /// <summary>
    /// The caller's vote on the player's current rating: <see langword="true"/> up, <see langword="false"/> down, or
    /// <see langword="null"/> when they have not voted on it.
    /// </summary>
    public bool? MyVote { get; init; }

    /// <summary>
    /// The identifier of the caller's vote on the player's current rating, or <see langword="null"/> when they have not
    /// voted on it.
    /// </summary>
    public int? MyVoteId { get; init; }
}
