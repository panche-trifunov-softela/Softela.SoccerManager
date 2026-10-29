using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetMyLeagueTeamManagerApplications;

/// <summary>
/// Represents a request to retrieve the currently authenticated user's pending league team manager applications that can still be accepted.
/// </summary>
public sealed record GetMyLeagueTeamManagerApplicationsRequest : IRequest<GetMyLeagueTeamManagerApplicationsResponse>;
