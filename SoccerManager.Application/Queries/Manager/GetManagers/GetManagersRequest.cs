using MediatR;

namespace SoccerManager.Application.Queries.Manager.GetManagers;

/// <summary>
/// Represents a request to retrieve all managers.
/// </summary>
public sealed record GetManagersRequest : IRequest<GetManagersResponse>;
