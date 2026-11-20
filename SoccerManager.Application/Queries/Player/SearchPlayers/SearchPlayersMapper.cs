using SoccerManager.Application.Dtos.PlayerSearch;
using SoccerManager.Application.Models.SearchPlayers;

namespace SoccerManager.Application.Queries.Player.SearchPlayers;

/// <summary>
/// Maps each row of a player search result to a <see cref="PlayerSearchItemDto"/>.
/// </summary>
public static class SearchPlayersMapper
{
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
            PositionShortName = row.PositionShortName,
            MyVote = row.MyVote,
            MyVoteId = row.MyVoteId,
        };
    }
}
