namespace SoccerManager.Application.Models.SearchPlayers;

/// <summary>
/// One page of <see cref="SearchPlayersResult"/> rows from the <c>dbo.SearchPlayers</c> stored procedure, with the
/// number of players matching across all pages.
/// </summary>
public sealed record SearchPlayersPage
{
    /// <summary>
    /// The players on the requested page, in the requested order; empty for a page past the end.
    /// </summary>
    public required List<SearchPlayersResult> Rows { get; init; }

    /// <summary>
    /// The number of players matching the search across all pages.
    /// </summary>
    public int TotalCount { get; init; }
}
