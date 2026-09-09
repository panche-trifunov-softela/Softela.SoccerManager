using MediatR;

namespace SoccerManager.Application.Commands.Division.DeleteDivision;

/// <summary>
/// Represents a request to delete an existing division.
/// </summary>
public sealed record DeleteDivisionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the division to delete.
    /// </summary>
    public int Id { get; init; }
}
