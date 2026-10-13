using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;

/// <summary>
/// Represents the result of a <see cref="GetAvailableLeagueTeamsRequest"/> query.
/// </summary>
public sealed record GetAvailableLeagueTeamsResponse
{
    /// <summary>
    /// Every club on the requested league's roster, by team name, with whether each one has a current manager.
    /// </summary>
    public required List<AvailableLeagueTeamDto> Data { get; init; }
}
