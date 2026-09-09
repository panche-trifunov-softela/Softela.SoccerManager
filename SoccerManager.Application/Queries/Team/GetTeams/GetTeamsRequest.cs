using MediatR;

namespace SoccerManager.Application.Queries.Team.GetTeams;

/// <summary>
/// Represents a request to retrieve all teams.
/// </summary>
public sealed record GetTeamsRequest : IRequest<GetTeamsResponse>;
