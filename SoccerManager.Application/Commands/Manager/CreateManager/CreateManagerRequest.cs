using MediatR;

namespace SoccerManager.Application.Commands.Manager.CreateManager;

/// <summary>
/// Represents a request to create a new manager profile.
/// </summary>
public sealed record CreateManagerRequest : IRequest<int>
{
    /// <summary>
    /// The URL of the manager's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }
}
