using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplications;

/// <summary>
/// Represents a request to retrieve every application belonging to a league and team.
/// </summary>
public sealed record GetLeagueTeamManagerApplicationsRequest : IRequest<GetLeagueTeamManagerApplicationsResponse>
{
    /// <summary>
    /// The identifier of the league whose applications are being requested.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The identifier of the team whose applications are being requested.
    /// </summary>
    public int TeamId { get; init; }
}
