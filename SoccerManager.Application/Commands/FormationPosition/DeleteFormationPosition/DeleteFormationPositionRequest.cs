using MediatR;

namespace SoccerManager.Application.Commands.FormationPosition.DeleteFormationPosition;

/// <summary>
/// Represents a request to delete an existing formation position slot.
/// </summary>
public sealed record DeleteFormationPositionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the formation position slot to delete.
    /// </summary>
    public int Id { get; init; }
}
