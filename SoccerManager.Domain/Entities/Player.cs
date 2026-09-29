namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a player who can be part of a team's squad.
/// </summary>
public class Player : BaseEntity
{
    /// <summary>
    /// Gets or sets the player's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the player's date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the player's overall rating.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Gets or sets the player's market value.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Gets or sets the player's wage.
    /// </summary>
    public decimal Wage { get; set; }

    /// <summary>
    /// Gets or sets the URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public int? NationalTeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player's current real-life club, or <see langword="null"/> when it has none.
    /// </summary>
    public int? TeamId { get; set; }
}
