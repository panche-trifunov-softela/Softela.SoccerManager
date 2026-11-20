namespace SoccerManager.Application.Core.PlayerSearch;

/// <summary>
/// The filters, sort order and page a player search request offers, every one optional. The players search and the
/// league player search take the same ones, so they share their validation and their defaults.
/// </summary>
public interface IPlayerSearchFilters
{
    /// <summary>
    /// The text a player name must contain, ignoring case and accents, or <see langword="null"/> for no name filter.
    /// </summary>
    string? SearchTerm { get; }

    /// <summary>
    /// The identifier of the main position a player must have, or <see langword="null"/> for any position.
    /// </summary>
    int? PositionId { get; }

    /// <summary>
    /// The identifier of the club a player must have, or <see langword="null"/> for any club: the current real-life club
    /// for the players search; for the league player search, the club of the registration shown for him in the league
    /// (the searching club's own first, else his lowest), so a free agent never matches.
    /// </summary>
    int? TeamId { get; }

    /// <summary>
    /// The identifier of the national team a player must have, or <see langword="null"/> for any national team.
    /// </summary>
    int? NationalTeamId { get; }

    /// <summary>
    /// The lowest rating to include, or <see langword="null"/> for no lower bound.
    /// </summary>
    int? MinRating { get; }

    /// <summary>
    /// The highest rating to include, or <see langword="null"/> for no upper bound.
    /// </summary>
    int? MaxRating { get; }

    /// <summary>
    /// The column to order by (name, rating, age, value or wage, in any case), or <see langword="null"/> for rating.
    /// </summary>
    string? SortBy { get; }

    /// <summary>
    /// Whether to order in descending order, or <see langword="null"/> for descending.
    /// </summary>
    bool? SortDescending { get; }

    /// <summary>
    /// The 1-based page to return, or <see langword="null"/> for the first page.
    /// </summary>
    int? Page { get; }

    /// <summary>
    /// The number of players per page, or <see langword="null"/> for 25.
    /// </summary>
    int? PageSize { get; }
}
