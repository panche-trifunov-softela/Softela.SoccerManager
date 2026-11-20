using SoccerManager.Application.Dtos.PlayersInLeague;

namespace SoccerManager.Application.Queries.LeagueTeam.SearchPlayersInLeague;

/// <summary>
/// Represents the result of a <see cref="SearchPlayersInLeagueRequest"/> query.
/// </summary>
public sealed record SearchPlayersInLeagueResponse
{
    /// <summary>
    /// The requested page of matching players, with the total match count and the page and page size applied.
    /// </summary>
    public required PlayersInLeagueResultDto Data { get; init; }
}
