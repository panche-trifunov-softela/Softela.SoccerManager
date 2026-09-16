using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagers;

/// <summary>
/// Represents a request to retrieve every manager appointment belonging to a league and team.
/// </summary>
public sealed record GetLeagueTeamManagersRequest : IRequest<GetLeagueTeamManagersResponse>
{
    /// <summary>
    /// The identifier of the league whose manager appointments are being requested.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The identifier of the team whose manager appointments are being requested.
    /// </summary>
    public int TeamId { get; init; }
}
