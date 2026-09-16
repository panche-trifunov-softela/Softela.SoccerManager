using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagers;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamManagersRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamManagersResponse
{
    /// <summary>
    /// The list of manager appointments belonging to the requested league and team.
    /// </summary>
    public required List<LeagueTeamManagerDto> Data { get; init; }
}
