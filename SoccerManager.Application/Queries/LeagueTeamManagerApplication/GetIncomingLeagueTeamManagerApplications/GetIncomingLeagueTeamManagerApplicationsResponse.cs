using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetIncomingLeagueTeamManagerApplications;

/// <summary>
/// Represents the result of a <see cref="GetIncomingLeagueTeamManagerApplicationsRequest"/> query.
/// </summary>
public sealed record GetIncomingLeagueTeamManagerApplicationsResponse
{
    /// <summary>
    /// The list of pending league team manager applications for clubs in the currently authenticated user's leagues.
    /// </summary>
    public required List<IncomingLeagueTeamManagerApplicationDto> Data { get; init; }
}
