using MediatR;

namespace SoccerManager.Application.Commands.Formation.CreateFormation;

/// <summary>
/// Represents a request to create a new formation.
/// </summary>
public sealed record CreateFormationRequest : IRequest<int>
{
    /// <summary>
    /// The name of the formation to create.
    /// </summary>
    public required string Name { get; init; }
}
