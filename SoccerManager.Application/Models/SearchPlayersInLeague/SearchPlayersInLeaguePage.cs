namespace SoccerManager.Application.Models.SearchPlayersInLeague;

/// <summary>
/// One page of <see cref="SearchPlayersInLeagueResult"/> rows from the <c>dbo.SearchPlayersInLeague</c> stored procedure,
/// with the number of players matching across all pages.
/// </summary>
public sealed record SearchPlayersInLeaguePage
{
    /// <summary>
    /// The players on the requested page, in the requested order; empty for a page past the end.
    /// </summary>
    public required List<SearchPlayersInLeagueResult> Rows { get; init; }

    /// <summary>
    /// The number of players matching the search across all pages.
    /// </summary>
    public int TotalCount { get; init; }
}
