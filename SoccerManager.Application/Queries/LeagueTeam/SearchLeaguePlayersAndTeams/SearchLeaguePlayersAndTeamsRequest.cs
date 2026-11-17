using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.SearchLeaguePlayersAndTeams;

/// <summary>
/// Represents a request to search the players and the clubs of the league of a league team.
/// </summary>
public sealed record SearchLeaguePlayersAndTeamsRequest : IRequest<SearchLeaguePlayersAndTeamsResponse>
{
    /// <summary>
    /// The identifier of the league team the caller is managing, whose league is the one to search.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The text a player's name or a club's name must contain, ignoring case and accents. The validator requires at least
    /// 2 characters once trimmed, so it is never <see langword="null"/> when the handler runs.
    /// </summary>
    public string? SearchTerm { get; init; }
}
