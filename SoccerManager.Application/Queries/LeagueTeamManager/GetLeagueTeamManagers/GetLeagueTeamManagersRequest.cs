using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagers;

/// <summary>
/// Represents a request to retrieve every manager appointment belonging to a league roster entry.
/// </summary>
public sealed record GetLeagueTeamManagersRequest : IRequest<GetLeagueTeamManagersResponse>
{
    /// <summary>
    /// The identifier of the league roster entry (the club in its league) whose manager appointments are being requested.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
