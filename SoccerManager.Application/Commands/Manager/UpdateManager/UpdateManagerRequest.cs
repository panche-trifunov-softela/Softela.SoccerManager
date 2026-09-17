using MediatR;

namespace SoccerManager.Application.Commands.Manager.UpdateManager;

/// <summary>
/// Represents a request to update an existing manager profile.
/// </summary>
public sealed record UpdateManagerRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the manager profile to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The URL of the manager's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }
}
