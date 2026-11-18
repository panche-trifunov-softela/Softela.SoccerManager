using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBidders;

/// <summary>
/// Maps a player's bid rows to <see cref="LeaguePlayerBidderDto"/> instances.
/// </summary>
public static class GetLeaguePlayerBiddersMapper
{
    /// <summary>
    /// Converts the bid rows into their DTO representation, keeping their order.
    /// </summary>
    /// <param name="rows">The bid rows returned by the stored procedure, in the order to keep.</param>
    /// <returns>The corresponding <see cref="LeaguePlayerBidderDto"/> list, in the order of <paramref name="rows"/>.</returns>
    public static List<LeaguePlayerBidderDto> ToDtos(IReadOnlyList<GetLeaguePlayerBiddersResult> rows)
    {
        return rows.Select(ToDto).ToList();
    }

    /// <summary>
    /// Converts a bid row into its DTO representation.
    /// </summary>
    /// <param name="row">The bid row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeaguePlayerBidderDto"/>.</returns>
    public static LeaguePlayerBidderDto ToDto(GetLeaguePlayerBiddersResult row)
    {
        return new LeaguePlayerBidderDto
        {
            Id = row.Id,
            IsLoan = row.IsLoan,
            Status = row.Status,
            LeagueTeamId = row.LeagueTeamId,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            ManagerName = row.ManagerName,
            SeasonNumber = row.SeasonNumber,
            CreatedAt = row.CreatedAt,
        };
    }
}
