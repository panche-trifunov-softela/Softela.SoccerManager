using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamNextMatch;

/// <summary>
/// Represents a request to retrieve the next match of a league team, with the club's lineup for it.
/// </summary>
public sealed record GetLeagueTeamNextMatchRequest : IRequest<GetLeagueTeamNextMatchResponse>
{
    /// <summary>
    /// The identifier of the league team (the club in its league) whose next match is being requested.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
