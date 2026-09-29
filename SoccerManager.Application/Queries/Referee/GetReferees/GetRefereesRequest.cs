using MediatR;

namespace SoccerManager.Application.Queries.Referee.GetReferees;

/// <summary>
/// Represents a request to retrieve all referees.
/// </summary>
public sealed record GetRefereesRequest : IRequest<GetRefereesResponse>;
