using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Position.CreatePosition;

/// <summary>
/// Represents a request to create a new position.
/// </summary>
public sealed record CreatePositionRequest : IRequest<int>
{
    /// <summary>
    /// The name of the position to create.
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
