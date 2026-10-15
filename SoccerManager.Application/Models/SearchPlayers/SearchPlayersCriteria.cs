using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models.SearchPlayers;

/// <summary>
/// The filters, sort order and page of a player search, with every default already applied.
/// </summary>
public sealed record SearchPlayersCriteria
{
    /// <summary>
    /// The text a player name must contain, or <see langword="null"/> for no name filter.
    /// </summary>
    public string? SearchTerm { get; init; }

    /// <summary>
    /// The identifier of the main position a player must have, or <see langword="null"/> for any position.
    /// </summary>
    public int? PositionId { get; init; }

    /// <summary>
    /// The identifier of the current real-life club a player must have, or <see langword="null"/> for any club.
    /// </summary>
    public int? TeamId { get; init; }

    /// <summary>
    /// The identifier of the national team a player must have, or <see langword="null"/> for any national team.
    /// </summary>
    public int? NationalTeamId { get; init; }

    /// <summary>
    /// The lowest rating to include, or <see langword="null"/> for no lower bound.
    /// </summary>
    public int? MinRating { get; init; }

    /// <summary>
    /// The highest rating to include, or <see langword="null"/> for no upper bound.
    /// </summary>
    public int? MaxRating { get; init; }

    /// <summary>
    /// The column the players are ordered by.
    /// </summary>
    public PlayerSortBy SortBy { get; init; }

    /// <summary>
    /// Whether the players are ordered in descending order.
    /// </summary>
    public bool SortDescending { get; init; }

    /// <summary>
    /// The 1-based page to return.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// The number of players per page.
    /// </summary>
    public int PageSize { get; init; }
}
