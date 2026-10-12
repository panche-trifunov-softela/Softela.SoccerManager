using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeams;

/// <summary>
/// Represents a request to retrieve every team on the roster of a league.
/// </summary>
public sealed record GetLeagueTeamsRequest : IRequest<GetLeagueTeamsResponse>
{
    /// <summary>
    /// The identifier of the league whose roster is being requested.
    /// </summary>
    public int LeagueId { get; init; }
}
