using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetFormationSlots</c> stored procedure: a slot of a formation with its position and its
/// place on the pitch. Dapper populates a result by setting properties, so the row is its own class rather than a domain
/// entity.
/// </summary>
public class GetFormationSlotsResult
{
    /// <summary>
    /// The identifier of the formation.
    /// </summary>
    public int FormationId { get; set; }

    /// <summary>
    /// The name of the formation.
    /// </summary>
    public string FormationName { get; set; } = string.Empty;

    /// <summary>
    /// The identifier of the formation slot.
    /// </summary>
    public int FormationPositionId { get; set; }

    /// <summary>
    /// The number of the slot, from 1 to 19: 1 to 11 are the starters and 12 to 19 the reserves.
    /// </summary>
    public int SlotNumber { get; set; }

    /// <summary>
    /// The identifier of the position the slot asks for, or <see langword="null"/> for a reserve slot.
    /// </summary>
    public int? PositionId { get; set; }

    /// <summary>
    /// The name of the slot's position, or <see langword="null"/> for a reserve slot.
    /// </summary>
    public string? PositionName { get; set; }

    /// <summary>
    /// The short name of the slot's position, for example CB, or <see langword="null"/> for a reserve slot.
    /// </summary>
    public string? PositionShortName { get; set; }

    /// <summary>
    /// The area of the pitch the slot's position occupies, or <see langword="null"/> for a reserve slot.
    /// </summary>
    public PositionArea? PositionArea { get; set; }

    /// <summary>
    /// How far across the pitch the slot sits, from 0 (the team's left touchline) to 100 (its right), or
    /// <see langword="null"/> for a reserve slot and for a starter slot that has not been placed yet.
    /// </summary>
    public byte? PitchX { get; set; }

    /// <summary>
    /// How far up the pitch the slot sits, from 0 (the team's own goal line) to 100 (the opponent's), or
    /// <see langword="null"/> for a reserve slot and for a starter slot that has not been placed yet.
    /// </summary>
    public byte? PitchY { get; set; }
}
