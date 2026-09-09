using MediatR;

namespace SoccerManager.Application.Commands.Position.DeletePosition;

/// <summary>
/// Represents a request to delete an existing position.
/// </summary>
public sealed record DeletePositionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the position to delete.
    /// </summary>
    public int Id { get; init; }
}
