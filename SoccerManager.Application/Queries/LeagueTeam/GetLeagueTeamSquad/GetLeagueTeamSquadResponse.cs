using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquad;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamSquadRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamSquadResponse
{
    /// <summary>
    /// The requested league team's squad, ordered by line and position, with each player's current rating and main position.
    /// </summary>
    public required List<LeagueTeamSquadPlayerDto> Data { get; init; }
}
