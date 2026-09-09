using MediatR;

namespace SoccerManager.Application.Queries.Position.GetPositions;

/// <summary>
/// Represents a request to retrieve all positions.
/// </summary>
public sealed record GetPositionsRequest : IRequest<GetPositionsResponse>;
