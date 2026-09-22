using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetMyLeagueTeamManagers;

/// <summary>
/// Represents the result of a <see cref="GetMyLeagueTeamManagersRequest"/> query.
/// </summary>
public sealed record GetMyLeagueTeamManagersResponse
{
    /// <summary>
    /// The list of league team manager appointments belonging to the currently authenticated user.
    /// </summary>
    public required List<MyLeagueTeamManagerDto> Data { get; init; }
}
