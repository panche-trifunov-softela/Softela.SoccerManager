using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplications;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamManagerApplicationsRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamManagerApplicationsResponse
{
    /// <summary>
    /// The applications belonging to the requested league and team, newest first.
    /// </summary>
    public required List<LeagueTeamManagerApplicationDto> Data { get; init; }
}
