using MediatR;

namespace SoccerManager.Application.Commands.Formation.UpdateFormation;

/// <summary>
/// Represents a request to update an existing formation.
/// </summary>
public sealed record UpdateFormationRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the formation to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the formation.
    /// </summary>
    public required string Name { get; init; }
}
