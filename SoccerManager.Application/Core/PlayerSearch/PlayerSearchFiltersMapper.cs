using SoccerManager.Application.Models.SearchPlayers;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Core.PlayerSearch;

/// <summary>
/// Converts the filters, sort order and page every player search request offers into <see cref="SearchPlayersCriteria"/>,
/// with the defaults applied.
/// </summary>
public static class PlayerSearchFiltersMapper
{
    /// <summary>
    /// The page returned when the request names none.
    /// </summary>
    private const int DefaultPage = 1;

    /// <summary>
    /// The number of players per page when the request names none.
    /// </summary>
    private const int DefaultPageSize = 25;

    /// <summary>
    /// The column the players are ordered by when the request names none.
    /// </summary>
    private const PlayerSortBy DefaultSortBy = PlayerSortBy.Rating;

    /// <summary>
    /// Whether the players are ordered in descending order when the request does not say, so the best come first.
    /// </summary>
    private const bool DefaultSortDescending = true;

    /// <summary>
    /// Converts validated filters into the criteria the repository takes, filling in the defaults.
    /// </summary>
    /// <param name="filters">The validated filters of a search request.</param>
    /// <returns>The corresponding <see cref="SearchPlayersCriteria"/>.</returns>
    public static SearchPlayersCriteria ToCriteria(IPlayerSearchFilters filters)
    {
        // A blank term means no name filter. It must not reach the procedure as an
        // empty string, which CHARINDEX would match against nothing.
        var searchTerm = string.IsNullOrWhiteSpace(filters.SearchTerm) ? null : filters.SearchTerm.Trim();

        // The validator has already limited SortBy to the enum's names.
        var sortBy = filters.SortBy is null ? DefaultSortBy : Enum.Parse<PlayerSortBy>(filters.SortBy, ignoreCase: true);

        return new SearchPlayersCriteria
        {
            SearchTerm = searchTerm,
            PositionId = filters.PositionId,
            TeamId = filters.TeamId,
            NationalTeamId = filters.NationalTeamId,
            MinRating = filters.MinRating,
            MaxRating = filters.MaxRating,
            SortBy = sortBy,
            SortDescending = filters.SortDescending ?? DefaultSortDescending,
            Page = filters.Page ?? DefaultPage,
            PageSize = filters.PageSize ?? DefaultPageSize,
        };
    }
}
