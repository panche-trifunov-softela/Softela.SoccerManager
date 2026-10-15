using SoccerManager.Application.Dtos.PlayerSearch;

namespace SoccerManager.Application.Queries.Player.SearchPlayers;

/// <summary>
/// Represents the result of a <see cref="SearchPlayersRequest"/> query.
/// </summary>
public sealed record SearchPlayersResponse
{
    /// <summary>
    /// The requested page of matching players, with the total match count and the page and page size applied.
    /// </summary>
    public required PlayerSearchResultDto Data { get; init; }
}
