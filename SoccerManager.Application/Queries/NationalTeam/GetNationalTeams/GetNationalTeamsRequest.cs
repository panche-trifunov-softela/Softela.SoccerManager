using MediatR;

namespace SoccerManager.Application.Queries.NationalTeam.GetNationalTeams;

/// <summary>
/// Represents a request to retrieve all national teams.
/// </summary>
public sealed record GetNationalTeamsRequest : IRequest<GetNationalTeamsResponse>;
