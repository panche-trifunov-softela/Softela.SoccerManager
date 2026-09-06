using MediatR;

namespace SoccerManager.Application.Queries.League.GetLeagues;

/// <summary>
/// Represents a request to retrieve all leagues.
/// </summary>
public sealed record GetLeaguesRequest : IRequest<GetLeaguesResponse>;
