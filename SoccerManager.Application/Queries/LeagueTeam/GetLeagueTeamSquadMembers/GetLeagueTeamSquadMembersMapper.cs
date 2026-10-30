using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquadMembers;

/// <summary>
/// Maps squad entry rows to <see cref="LeagueTeamSquadMemberDto"/> instances.
/// </summary>
public static class GetLeagueTeamSquadMembersMapper
{
    /// <summary>
    /// Converts a squad entry row into its DTO representation.
    /// </summary>
    /// <param name="row">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="LeagueTeamSquadMemberDto"/>.</returns>
    public static LeagueTeamSquadMemberDto ToDto(GetLeagueTeamSquadMembersResult row)
    {
        return new LeagueTeamSquadMemberDto
        {
            Id = row.Id,
            PlayerId = row.PlayerId,
            Name = row.Name,
            ImageUrl = row.ImageUrl,
            DateOfBirth = row.DateOfBirth,
            SquadNumber = row.SquadNumber,
            PositionId = row.PositionId,
            PositionName = row.PositionName,
            PositionShortName = row.PositionShortName,
            NationalTeamId = row.NationalTeamId,
            NationalTeamName = row.NationalTeamName,
            NationalTeamLogoUrl = row.NationalTeamLogoUrl,
            Rating = row.Rating,
            Value = row.Value,
            Wage = row.Wage,
            Morale = row.Morale,
            Condition = row.Condition,
        };
    }
}
