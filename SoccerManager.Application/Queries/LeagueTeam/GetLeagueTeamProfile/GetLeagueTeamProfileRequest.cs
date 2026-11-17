using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamProfile;

/// <summary>
/// Represents a request to retrieve the profile of a league team.
/// </summary>
public sealed record GetLeagueTeamProfileRequest : IRequest<GetLeagueTeamProfileResponse>
{
    /// <summary>
    /// The identifier of the league team whose profile is being requested.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
