using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquadMembers;

/// <summary>
/// Represents a request to retrieve the squad entries of a league team.
/// </summary>
public sealed record GetLeagueTeamSquadMembersRequest : IRequest<GetLeagueTeamSquadMembersResponse>
{
    /// <summary>
    /// The identifier of the league team whose squad is being requested.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
