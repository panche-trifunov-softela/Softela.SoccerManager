using MediatR;

namespace SoccerManager.Application.Commands.FormationPosition.CreateFormationPosition;

/// <summary>
/// Represents a request to create a new formation position slot.
/// </summary>
public sealed record CreateFormationPositionRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the formation the slot belongs to.
    /// </summary>
    public int FormationId { get; init; }

    /// <summary>
    /// The identifier of the position filling the slot.
    /// </summary>
    public int PositionId { get; init; }

    /// <summary>
    /// The slot the position fills within the formation, from 1 to 11.
    /// </summary>
    public int SlotNumber { get; init; }
}
