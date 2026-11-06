namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetMatchLineupRows</c> stored procedure: a stored lineup slot of a club for a match,
/// joined to its formation slot and to the player behind the position rating it uses. Dapper populates a result by
/// setting properties, so the row is its own class rather than a domain entity.
/// </summary>
public class GetMatchLineupRowsResult
{
    /// <summary>
    /// The identifier of the stored lineup slot.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The identifier of the formation the slot belongs to.
    /// </summary>
    public int FormationId { get; set; }

    /// <summary>
    /// The identifier of the formation slot the row fills.
    /// </summary>
    public int FormationPositionId { get; set; }

    /// <summary>
    /// The number of the formation slot, from 1 to 19: 1 to 11 are the starters and 12 to 19 the reserves.
    /// </summary>
    public int SlotNumber { get; set; }

    /// <summary>
    /// The identifier of the position the slot asks for, or <see langword="null"/> for a reserve slot.
    /// </summary>
    public int? SlotPositionId { get; set; }

    /// <summary>
    /// The identifier of the player position rating the row uses.
    /// </summary>
    public int PlayerPositionId { get; set; }

    /// <summary>
    /// The identifier of the player the row seats.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// The identifier of the position of the rating the row uses, which may differ from the position the slot asks for.
    /// </summary>
    public int PlayedPositionId { get; set; }

    /// <summary>
    /// The player's condition when the row was written.
    /// </summary>
    public int ConditionOnMatch { get; set; }

    /// <summary>
    /// The player's quality for the slot when the row was written.
    /// </summary>
    public int QualityAtPositionOnMatch { get; set; }

    /// <summary>
    /// Whether the player was suspended when the row was written.
    /// </summary>
    public bool IsSuspended { get; set; }
}
