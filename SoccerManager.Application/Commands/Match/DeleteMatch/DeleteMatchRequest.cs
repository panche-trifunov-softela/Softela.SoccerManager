using MediatR;

namespace SoccerManager.Application.Commands.Match.DeleteMatch;

/// <summary>
/// Represents a request to delete an existing match.
/// </summary>
public sealed record DeleteMatchRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match to delete.
    /// </summary>
    public int Id { get; init; }
}
