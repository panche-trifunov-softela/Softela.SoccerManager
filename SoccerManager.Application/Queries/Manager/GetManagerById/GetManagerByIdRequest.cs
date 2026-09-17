using MediatR;

namespace SoccerManager.Application.Queries.Manager.GetManagerById;

/// <summary>
/// Represents a request to retrieve a single manager profile by identifier.
/// </summary>
public sealed record GetManagerByIdRequest : IRequest<GetManagerByIdResponse>
{
    /// <summary>
    /// The identifier of the manager profile to retrieve.
    /// </summary>
    public int Id { get; init; }
}
