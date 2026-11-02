namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one of a formation's slots and the position that fills it (a starter slot has one, a reserve slot none).
/// </summary>
public class FormationPosition : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the formation the slot belongs to.
    /// </summary>
    public int FormationId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the position filling the slot, or <see langword="null"/> for a reserve slot (12 to 19), which has no fixed position.
    /// </summary>
    public int? PositionId { get; set; }

    /// <summary>
    /// Gets or sets the slot within the formation, from 1 to 19: 1 to 11 are the starters and 12 to 19 the reserves.
    /// </summary>
    public int SlotNumber { get; set; }

    /// <summary>
    /// Gets or sets how far across the pitch the slot sits, from 0 (the team's left touchline) to 100 (its right), or <see langword="null"/> for a reserve slot and for a starter slot that has not been placed yet.
    /// </summary>
    public byte? PitchX { get; set; }

    /// <summary>
    /// Gets or sets how far up the pitch the slot sits, from 0 (the team's own goal line) to 100 (the opponent's), or <see langword="null"/> for a reserve slot and for a starter slot that has not been placed yet.
    /// </summary>
    public byte? PitchY { get; set; }
}
