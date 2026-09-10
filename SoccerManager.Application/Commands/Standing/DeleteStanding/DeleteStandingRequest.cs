using MediatR;

namespace SoccerManager.Application.Commands.Standing.DeleteStanding;

/// <summary>
/// Represents a request to delete an existing standing.
/// </summary>
public sealed record DeleteStandingRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the standing to delete.
    /// </summary>
    public int Id { get; init; }
}
