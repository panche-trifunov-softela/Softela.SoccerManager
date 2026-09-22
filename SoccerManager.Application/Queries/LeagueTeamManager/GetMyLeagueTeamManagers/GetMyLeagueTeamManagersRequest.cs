using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetMyLeagueTeamManagers;

/// <summary>
/// Represents a request to retrieve the currently authenticated user's league team manager
/// appointments.
/// </summary>
public sealed record GetMyLeagueTeamManagersRequest : IRequest<GetMyLeagueTeamManagersResponse>
{
    /// <summary>
    /// Whether to narrow the result to appointments that are still current.
    /// </summary>
    public bool CurrentOnly { get; init; }
}
