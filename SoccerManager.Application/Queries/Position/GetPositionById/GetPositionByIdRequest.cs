using MediatR;

namespace SoccerManager.Application.Queries.Position.GetPositionById;

/// <summary>
/// Represents a request to retrieve a single position by identifier.
/// </summary>
public sealed record GetPositionByIdRequest : IRequest<GetPositionByIdResponse>
{
    /// <summary>
    /// The identifier of the position to retrieve.
    /// </summary>
    public int Id { get; init; }
}
