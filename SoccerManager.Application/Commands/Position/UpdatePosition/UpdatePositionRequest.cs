using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Position.UpdatePosition;

/// <summary>
/// Represents a request to update an existing position.
/// </summary>
public sealed record UpdatePositionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the position to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the position.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The area of the pitch the position plays in.
    /// </summary>
    public PositionArea Area { get; init; }

    /// <summary>
    /// The side of the pitch the position plays on.
    /// </summary>
    public PositionSide Side { get; init; }
}
