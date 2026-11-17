using SoccerManager.Application.Dtos.LeagueSearch;

namespace SoccerManager.Application.Queries.LeagueTeam.SearchLeaguePlayersAndTeams;

/// <summary>
/// Represents the result of a <see cref="SearchLeaguePlayersAndTeamsRequest"/> query.
/// </summary>
public sealed record SearchLeaguePlayersAndTeamsResponse
{
    /// <summary>
    /// The best matching players and clubs of the league, each with the number matching in all.
    /// </summary>
    public required LeagueSearchResultDto Data { get; init; }
}
