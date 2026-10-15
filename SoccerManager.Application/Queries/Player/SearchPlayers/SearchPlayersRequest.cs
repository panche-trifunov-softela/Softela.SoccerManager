using MediatR;

namespace SoccerManager.Application.Queries.Player.SearchPlayers;

/// <summary>
/// Represents a request to retrieve one page of the players matching a search, filters and sort order.
/// Every member is optional; the handler fills in the defaults.
/// </summary>
public sealed record SearchPlayersRequest : IRequest<SearchPlayersResponse>
{
    /// <summary>
    /// The text a player name must contain, ignoring case and accents, or <see langword="null"/> for no name filter.
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
    /// The column to order by (name, rating, age, value or wage, in any case), or <see langword="null"/> for rating.
    /// </summary>
    public string? SortBy { get; init; }

    /// <summary>
    /// Whether to order in descending order, or <see langword="null"/> for descending.
    /// </summary>
    public bool? SortDescending { get; init; }

    /// <summary>
    /// The 1-based page to return, or <see langword="null"/> for the first page.
    /// </summary>
    public int? Page { get; init; }

    /// <summary>
    /// The number of players per page, or <see langword="null"/> for 25.
    /// </summary>
    public int? PageSize { get; init; }
}
