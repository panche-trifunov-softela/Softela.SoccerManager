namespace SoccerManager.Application.Dtos.PlayerSearch;

/// <summary>
/// Represents one page of a players search, with the total number of matching players.
/// </summary>
public sealed record PlayerSearchResultDto
{
    /// <summary>
    /// The players on the requested page, in the requested order; empty for a page past the end.
    /// </summary>
    public required List<PlayerSearchItemDto> Items { get; init; }

    /// <summary>
    /// The number of players matching the search across all pages.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// The 1-based page returned.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// The number of players per page that was applied.
    /// </summary>
    public int PageSize { get; init; }
}
