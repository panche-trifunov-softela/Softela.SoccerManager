using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeams;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamsRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamsResponse
{
    /// <summary>
    /// The list of teams on the requested league's roster.
    /// </summary>
    public required List<LeagueTeamDto> Data { get; init; }
}
