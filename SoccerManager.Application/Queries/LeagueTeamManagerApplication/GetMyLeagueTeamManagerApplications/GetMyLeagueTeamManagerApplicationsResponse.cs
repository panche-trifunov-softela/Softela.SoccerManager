using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetMyLeagueTeamManagerApplications;

/// <summary>
/// Represents the result of a <see cref="GetMyLeagueTeamManagerApplicationsRequest"/> query.
/// </summary>
public sealed record GetMyLeagueTeamManagerApplicationsResponse
{
    /// <summary>
    /// The list of pending league team manager applications belonging to the currently authenticated user.
    /// </summary>
    public required List<MyLeagueTeamManagerApplicationDto> Data { get; init; }
}
