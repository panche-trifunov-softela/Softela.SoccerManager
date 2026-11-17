using SoccerManager.Application.Dtos.LeagueSearch;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.SearchLeaguePlayersAndTeams;

/// <summary>
/// Maps the types of a league search: the request's term to the text searched for, and each result row to its DTO.
/// </summary>
public static class SearchLeaguePlayersAndTeamsMapper
{
    /// <summary>
    /// Converts the request's search term into the text the procedure searches for.
    /// </summary>
    /// <param name="term">The search term of a validated request, which is never <see langword="null"/>.</param>
    /// <returns>The term without the whitespace around it.</returns>
    public static string ToSearchTerm(string? term)
    {
        // The validator has already required a term of at least 2 characters once trimmed.
        return term!.Trim();
    }

    /// <summary>
    /// Converts a player row into its DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeagueSearchPlayerItemDto"/>.</returns>
    public static LeagueSearchPlayerItemDto ToPlayerDto(SearchLeaguePlayersResult row)
    {
        return new LeagueSearchPlayerItemDto
        {
            PlayerId = row.PlayerId,
            Name = row.Name,
            ImageUrl = row.ImageUrl,
            Rating = row.Rating,
            PositionName = row.PositionName,
            PositionShortName = row.PositionShortName,
            LeagueTeamId = row.LeagueTeamId,
            TeamName = row.TeamName,
        };
    }

    /// <summary>
    /// Converts a club row into its DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeagueSearchTeamItemDto"/>.</returns>
    public static LeagueSearchTeamItemDto ToTeamDto(SearchLeagueTeamsResult row)
    {
        return new LeagueSearchTeamItemDto
        {
            LeagueTeamId = row.LeagueTeamId,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
        };
    }
}
