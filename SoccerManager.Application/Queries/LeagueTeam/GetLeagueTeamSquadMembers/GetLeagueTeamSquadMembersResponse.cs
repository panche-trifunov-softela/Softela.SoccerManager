using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquadMembers;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamSquadMembersRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamSquadMembersResponse
{
    /// <summary>
    /// The requested league team's squad entries, ordered by line and position.
    /// </summary>
    public required List<LeagueTeamSquadMemberDto> Data { get; init; }
}
