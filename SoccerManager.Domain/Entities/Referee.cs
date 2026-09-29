using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a referee who can officiate a match.
/// </summary>
public class Referee : BaseEntity
{
    /// <summary>
    /// Gets or sets the referee's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL of the referee's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets how much foul play the referee lets go before penalizing it.
    /// </summary>
    public Tolerance Tolerance { get; set; }
}
