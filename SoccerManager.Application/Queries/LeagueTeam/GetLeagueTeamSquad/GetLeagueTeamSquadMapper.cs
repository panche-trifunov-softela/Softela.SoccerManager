using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquad;

/// <summary>
/// Maps squad rows to <see cref="LeagueTeamSquadPlayerDto"/> instances.
/// </summary>
public static class GetLeagueTeamSquadMapper
{
    /// <summary>
    /// Converts a squad row into its DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeagueTeamSquadPlayerDto"/>.</returns>
    public static LeagueTeamSquadPlayerDto ToDto(GetLeagueTeamSquadResult row)
    {
        return new LeagueTeamSquadPlayerDto
        {
            Id = row.Id,
            Name = row.Name,
            Rating = row.Rating,
            ImageUrl = row.ImageUrl,
            PositionId = row.PositionId,
            PositionName = row.PositionName,
        };
    }
}
