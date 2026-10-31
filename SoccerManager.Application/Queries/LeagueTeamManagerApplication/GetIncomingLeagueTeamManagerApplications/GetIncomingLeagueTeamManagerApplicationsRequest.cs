using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetIncomingLeagueTeamManagerApplications;

/// <summary>
/// Represents a request to retrieve the pending league team manager applications that can still be accepted, for clubs in
/// the leagues the currently authenticated user owns, or in every league for an operator.
/// </summary>
public sealed record GetIncomingLeagueTeamManagerApplicationsRequest : IRequest<GetIncomingLeagueTeamManagerApplicationsResponse>;
