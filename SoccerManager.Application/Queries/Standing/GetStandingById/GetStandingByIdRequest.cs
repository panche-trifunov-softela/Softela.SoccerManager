using MediatR;

namespace SoccerManager.Application.Queries.Standing.GetStandingById;

/// <summary>
/// Represents a request to retrieve a single standing by identifier.
/// </summary>
public sealed record GetStandingByIdRequest : IRequest<GetStandingByIdResponse>
{
    /// <summary>
    /// The identifier of the standing to retrieve.
    /// </summary>
    public int Id { get; init; }
}
