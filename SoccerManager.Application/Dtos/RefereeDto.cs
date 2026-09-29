using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a referee for read-oriented consumers.
/// </summary>
public sealed record RefereeDto
{
    /// <summary>
    /// The identifier of the referee.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the referee.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the referee's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// How much foul play the referee lets go before penalizing it.
    /// </summary>
    public Tolerance Tolerance { get; init; }

    /// <summary>
    /// The UTC date and time the referee was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the referee was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
