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
}
