namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a formation position slot for read-oriented consumers.
/// </summary>
public sealed record FormationPositionDto
{
    /// <summary>
    /// The identifier of the formation position slot.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the formation the slot belongs to.
    /// </summary>
    public int FormationId { get; init; }

    /// <summary>
    /// The identifier of the position filling the slot, or <see langword="null"/> for a reserve slot (12 to 19), which has no fixed position.
    /// </summary>
    public int? PositionId { get; init; }

    /// <summary>
    /// The slot within the formation, from 1 to 19: 1 to 11 are the starters and 12 to 19 the reserves.
    /// </summary>
    public int SlotNumber { get; init; }

    /// <summary>
    /// The UTC date and time the slot was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the slot was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
