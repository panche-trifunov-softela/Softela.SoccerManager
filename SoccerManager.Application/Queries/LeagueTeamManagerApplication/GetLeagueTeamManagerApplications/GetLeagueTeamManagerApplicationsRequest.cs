using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplications;

/// <summary>
/// Represents a request to retrieve every application belonging to a league roster entry.
/// </summary>
public sealed record GetLeagueTeamManagerApplicationsRequest : IRequest<GetLeagueTeamManagerApplicationsResponse>
{
    /// <summary>
    /// The identifier of the league roster entry (the club in its league) whose applications are being requested.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
