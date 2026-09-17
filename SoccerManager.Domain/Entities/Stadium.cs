namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a stadium that a team can play at.
/// </summary>
public class Stadium : BaseEntity
{
    /// <summary>
    /// Gets or sets the stadium's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL of the stadium's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the stadium's capacity, as a number of spectators.
    /// </summary>
    public int Size { get; set; }
}
