using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayers;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayersRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayersResponse
{
    /// <summary>
    /// The list of player registrations belonging to the requested league and team.
    /// </summary>
    public required List<LeagueTeamPlayerDto> Data { get; init; }
}
