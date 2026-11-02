using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamNextMatch;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamNextMatchRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamNextMatchResponse
{
    /// <summary>
    /// The club's next match, or <see langword="null"/> when it has none to come.
    /// </summary>
    public NextMatchDto? Data { get; init; }
}
