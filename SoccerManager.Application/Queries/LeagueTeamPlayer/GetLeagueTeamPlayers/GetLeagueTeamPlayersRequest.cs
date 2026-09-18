using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayers;

/// <summary>
/// Represents a request to retrieve every player registration belonging to a league and team.
/// </summary>
public sealed record GetLeagueTeamPlayersRequest : IRequest<GetLeagueTeamPlayersResponse>
{
    /// <summary>
    /// The identifier of the league whose player registrations are being requested.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The identifier of the team whose player registrations are being requested.
    /// </summary>
    public int TeamId { get; init; }
}
