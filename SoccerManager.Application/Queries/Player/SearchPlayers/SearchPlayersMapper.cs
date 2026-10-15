using SoccerManager.Application.Dtos.PlayerSearch;
using SoccerManager.Application.Models.SearchPlayers;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Queries.Player.SearchPlayers;

/// <summary>
/// Maps the types of a player search: the request to <see cref="SearchPlayersCriteria"/>, and each result row
/// to a <see cref="PlayerSearchItemDto"/>.
/// </summary>
public static class SearchPlayersMapper
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
    /// Converts a validated search request into the criteria the repository takes, filling in the defaults.
    /// </summary>
    /// <param name="request">The validated search request.</param>
    /// <returns>The corresponding <see cref="SearchPlayersCriteria"/>.</returns>
    public static SearchPlayersCriteria ToCriteria(SearchPlayersRequest request)
    {
        // A blank term means no name filter. It must not reach the procedure as an
        // empty string, which CHARINDEX would match against nothing.
        var searchTerm = string.IsNullOrWhiteSpace(request.SearchTerm) ? null : request.SearchTerm.Trim();

        // The validator has already limited SortBy to the enum's names.
        var sortBy = request.SortBy is null ? DefaultSortBy : Enum.Parse<PlayerSortBy>(request.SortBy, ignoreCase: true);

        return new SearchPlayersCriteria
        {
            SearchTerm = searchTerm,
            PositionId = request.PositionId,
            TeamId = request.TeamId,
            NationalTeamId = request.NationalTeamId,
            MinRating = request.MinRating,
            MaxRating = request.MaxRating,
            SortBy = sortBy,
            SortDescending = request.SortDescending ?? DefaultSortDescending,
            Page = request.Page ?? DefaultPage,
            PageSize = request.PageSize ?? DefaultPageSize,
        };
    }

    /// <summary>
    /// Converts a player search row into its DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="PlayerSearchItemDto"/>.</returns>
    public static PlayerSearchItemDto ToDto(SearchPlayersResult row)
    {
        return new PlayerSearchItemDto
        {
            Id = row.Id,
            Name = row.Name,
            DateOfBirth = row.DateOfBirth,
            Rating = row.Rating,
            Value = row.Value,
            Wage = row.Wage,
            ImageUrl = row.ImageUrl,
            TeamId = row.TeamId,
            TeamName = row.TeamName,
            NationalTeamId = row.NationalTeamId,
            NationalTeamName = row.NationalTeamName,
            PositionId = row.PositionId,
            PositionName = row.PositionName,
        };
    }
}
