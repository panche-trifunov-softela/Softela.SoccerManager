using MediatR;

namespace SoccerManager.Application.Commands.FormationPosition.UpdateFormationPosition;

/// <summary>
/// Represents a request to update an existing formation position slot.
/// </summary>
public sealed record UpdateFormationPositionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the formation position slot to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the formation the slot belongs to.
    /// </summary>
    public int FormationId { get; init; }

    /// <summary>
    /// The identifier of the position filling the slot: required for a starter slot (1 to 11), and <see langword="null"/> for a reserve slot (12 to 19).
    /// </summary>
    public int? PositionId { get; init; }

    /// <summary>
    /// The slot within the formation, from 1 to 19: 1 to 11 are the starters and 12 to 19 the reserves.
    /// </summary>
    public int SlotNumber { get; init; }

    /// <summary>
    /// How far across the pitch the slot sits, from 0 (the team's left touchline) to 100 (its right). Optional: sent together
    /// with <see cref="PitchY"/> and only for a starter slot (1 to 11); when omitted the slot keeps its stored place.
    /// </summary>
    public byte? PitchX { get; init; }

    /// <summary>
    /// How far up the pitch the slot sits, from 0 (the team's own goal line) to 100 (the opponent's). Optional: sent together
    /// with <see cref="PitchX"/> and only for a starter slot (1 to 11); when omitted the slot keeps its stored place.
    /// </summary>
    public byte? PitchY { get; init; }
}
