using MediatR;

namespace SoccerManager.Application.Queries.Stadium.GetStadiums;

/// <summary>
/// Represents a request to retrieve all stadiums.
/// </summary>
public sealed record GetStadiumsRequest : IRequest<GetStadiumsResponse>;
