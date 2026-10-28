using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquad;

/// <summary>
/// Represents a request to retrieve the squad of a league team.
/// </summary>
public sealed record GetLeagueTeamSquadRequest : IRequest<GetLeagueTeamSquadResponse>
{
    /// <summary>
    /// The identifier of the league team whose squad is being requested.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
