using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;

/// <summary>
/// Represents the result of a <see cref="GetAvailableLeagueTeamsRequest"/> query.
/// </summary>
public sealed record GetAvailableLeagueTeamsResponse
{
    /// <summary>
    /// The requested league's non-guest clubs, by team name, with whether each one has a current manager; guests are left out.
    /// </summary>
    public required List<AvailableLeagueTeamDto> Data { get; init; }
}
