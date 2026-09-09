namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a player for read-oriented consumers.
/// </summary>
public sealed record PlayerDto
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
    /// The UTC date and time the player was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the player was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
