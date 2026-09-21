using MediatR;

namespace SoccerManager.Application.Commands.Formation.DeleteFormation;

/// <summary>
/// Represents a request to delete an existing formation.
/// </summary>
public sealed record DeleteFormationRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the formation to delete.
    /// </summary>
    public int Id { get; init; }
}
