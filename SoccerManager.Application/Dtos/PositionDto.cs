using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a position for read-oriented consumers.
/// </summary>
public sealed record PositionDto
{
    /// <summary>
    /// The identifier of the position.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the position.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The area of the pitch the position occupies.
    /// </summary>
    public PositionArea Area { get; init; }

    /// <summary>
    /// The side of the pitch the position occupies.
    /// </summary>
    public PositionSide Side { get; init; }

    /// <summary>
    /// The UTC date and time the position was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the position was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
