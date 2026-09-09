using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a playing position that can be assigned to a player.
/// </summary>
public class Position : BaseEntity
{
    /// <summary>
    /// Gets or sets the position's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the area of the pitch the position occupies.
    /// </summary>
    public PositionArea Area { get; set; }

    /// <summary>
    /// Gets or sets the side of the pitch the position occupies.
    /// </summary>
    public PositionSide Side { get; set; }
}
