using MediatR;

namespace SoccerManager.Application.Commands.Manager.EnsureMyManager;

/// <summary>
/// Represents a request to make sure the authenticated caller has a manager profile, creating it when missing.
/// </summary>
public sealed record EnsureMyManagerRequest : IRequest<int>;
