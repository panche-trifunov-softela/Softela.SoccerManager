namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one of a formation's eleven slots and the position that fills it.
/// </summary>
public class FormationPosition : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the formation the slot belongs to.
    /// </summary>
    public int FormationId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the position filling the slot.
    /// </summary>
    public int PositionId { get; set; }

    /// <summary>
    /// Gets or sets the slot the position fills within the formation, from 1 to 11.
    /// </summary>
    public int SlotNumber { get; set; }
}
