using MediatR;

namespace SoccerManager.Application.Commands.Referee.DeleteReferee;

/// <summary>
/// Represents a request to delete an existing referee.
/// </summary>
public sealed record DeleteRefereeRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the referee to delete.
    /// </summary>
    public int Id { get; init; }
}
