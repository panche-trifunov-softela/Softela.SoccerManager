using SoccerManager.Application.Dtos.PlayersInLeague;
using SoccerManager.Application.Models.SearchPlayers;
using SoccerManager.Application.Models.SearchPlayersInLeague;

namespace SoccerManager.Application.Queries.LeagueTeam.SearchPlayersInLeague;

/// <summary>
/// Maps a page of a league player search to a <see cref="PlayersInLeagueResultDto"/>.
/// </summary>
public static class SearchPlayersInLeagueMapper
{
    /// <summary>
    /// Converts a page of league player rows into its DTO representation, echoing the page and page size applied.
    /// </summary>
    /// <param name="page">The page returned by the repository.</param>
    /// <param name="criteria">The criteria the page was read with, whose page and page size are echoed.</param>
    /// <returns>The corresponding <see cref="PlayersInLeagueResultDto"/>.</returns>
    public static PlayersInLeagueResultDto ToDto(SearchPlayersInLeaguePage page, SearchPlayersCriteria criteria)
    {
        return new PlayersInLeagueResultDto
        {
            Items = page.Rows.Select(ToItemDto).ToList(),
            TotalCount = page.TotalCount,
            Page = criteria.Page,
            PageSize = criteria.PageSize,
        };
    }

    /// <summary>
    /// Converts a league player row into its DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="PlayersInLeagueItemDto"/>.</returns>
    private static PlayersInLeagueItemDto ToItemDto(SearchPlayersInLeagueResult row)
    {
        return new PlayersInLeagueItemDto
        {
            PlayerId = row.PlayerId,
            Name = row.Name,
            DateOfBirth = row.DateOfBirth,
            Rating = row.Rating,
            Value = row.Value,
            Wage = row.Wage,
            ImageUrl = row.ImageUrl,
            LeagueTeamId = row.LeagueTeamId,
            TeamName = row.TeamName,
            NationalTeamId = row.NationalTeamId,
            NationalTeamName = row.NationalTeamName,
            PositionId = row.PositionId,
            PositionName = row.PositionName,
            PositionShortName = row.PositionShortName,
        };
    }
}
