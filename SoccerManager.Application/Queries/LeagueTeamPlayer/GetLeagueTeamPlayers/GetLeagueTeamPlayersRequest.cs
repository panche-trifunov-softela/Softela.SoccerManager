using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayers;

/// <summary>
/// Represents a request to retrieve every player registration belonging to a club's roster entry in a league.
/// </summary>
public sealed record GetLeagueTeamPlayersRequest : IRequest<GetLeagueTeamPlayersResponse>
{
    /// <summary>
    /// The identifier of the league team whose player registrations are being requested.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
