using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents one slot of a lineup, filled or empty.
/// </summary>
public sealed record MatchLineupSlotDto
{
    /// <summary>
    /// The number of the slot, from 1 to 19: 1 to 11 are the starters and 12 to 19 the reserves.
    /// </summary>
    public int SlotNumber { get; init; }

    /// <summary>
    /// The identifier of the position the slot asks for, or <see langword="null"/> for a reserve slot.
    /// </summary>
    public int? PositionId { get; init; }

    /// <summary>
    /// The name of the slot's position, or <see langword="null"/> for a reserve slot.
    /// </summary>
    public string? PositionName { get; init; }

    /// <summary>
    /// The short name of the slot's position, for example CB, or <see langword="null"/> for a reserve slot.
    /// </summary>
    public string? PositionShortName { get; init; }

    /// <summary>
    /// How far across the pitch the slot sits, from 0 (the team's left touchline) to 100 (its right), or
    /// <see langword="null"/> for a reserve slot and for a starter slot that has not been placed yet.
    /// </summary>
    public byte? PitchX { get; init; }

    /// <summary>
    /// How far up the pitch the slot sits, from 0 (the team's own goal line) to 100 (the opponent's), or
    /// <see langword="null"/> for a reserve slot and for a starter slot that has not been placed yet.
    /// </summary>
    public byte? PitchY { get; init; }

    /// <summary>
    /// The identifier of the player in the slot, or <see langword="null"/> when the slot is empty.
    /// </summary>
    public int? PlayerId { get; init; }

    /// <summary>
    /// The identifier of the player position rating the slot uses, or <see langword="null"/> when the slot is empty.
    /// </summary>
    public int? PlayerPositionId { get; init; }

    /// <summary>
    /// The identifier of the position of the rating the slot uses, which may differ from the position the slot asks for,
    /// or <see langword="null"/> when the slot is empty.
    /// </summary>
    public int? PlayedPositionId { get; init; }

    /// <summary>
    /// The player's condition when the row was written, or <see langword="null"/> when the slot is empty.
    /// </summary>
    public int? ConditionOnMatch { get; init; }

    /// <summary>
    /// The player's quality in the slot's position when the row was written, or <see langword="null"/> when the slot is empty. A
    /// reserve slot asks for no position, so it holds the quality of the player's main rating.
    /// </summary>
    public int? QualityAtPositionOnMatch { get; init; }

    /// <summary>
    /// How well the player suits the slot's position, worked out from the quality, or <see langword="null"/> for an empty slot
    /// and for a reserve slot, which asks for no position.
    /// </summary>
    public PositionFit? Fit { get; init; }

    /// <summary>
    /// Whether the player was suspended when the row was written, or <see langword="null"/> when the slot is empty.
    /// </summary>
    public bool? IsSuspended { get; init; }
}
