using MediatR;

namespace SoccerManager.Application.Queries.Match.GetMatches;

/// <summary>
/// Represents a request to retrieve all matches.
/// </summary>
public sealed record GetMatchesRequest : IRequest<GetMatchesResponse>;
