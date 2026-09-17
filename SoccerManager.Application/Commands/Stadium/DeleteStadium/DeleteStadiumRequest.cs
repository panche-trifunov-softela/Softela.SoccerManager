using MediatR;

namespace SoccerManager.Application.Commands.Stadium.DeleteStadium;

/// <summary>
/// Represents a request to delete an existing stadium.
/// </summary>
public sealed record DeleteStadiumRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the stadium to delete.
    /// </summary>
    public int Id { get; init; }
}
