using MediatR;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.DeleteMatchFormationPlayerPosition;

/// <summary>
/// Represents a request to delete an existing match formation player position.
/// </summary>
public sealed record DeleteMatchFormationPlayerPositionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match formation player position to delete.
    /// </summary>
    public int Id { get; init; }
}
