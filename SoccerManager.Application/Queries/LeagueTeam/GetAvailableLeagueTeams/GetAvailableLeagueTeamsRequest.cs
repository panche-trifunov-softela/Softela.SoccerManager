using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;

/// <summary>
/// Represents a request to retrieve every club on a league's roster, with whether each one currently has a manager.
/// </summary>
public sealed record GetAvailableLeagueTeamsRequest : IRequest<GetAvailableLeagueTeamsResponse>
{
    /// <summary>
    /// The identifier of the league whose available clubs are being requested.
    /// </summary>
    public int LeagueId { get; init; }
}
