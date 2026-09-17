using MediatR;

namespace SoccerManager.Application.Commands.Manager.DeleteManager;

/// <summary>
/// Represents a request to delete an existing manager profile.
/// </summary>
public sealed record DeleteManagerRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the manager profile to delete.
    /// </summary>
    public int Id { get; init; }
}
