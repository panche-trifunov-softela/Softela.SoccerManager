using SoccerManager.Application.Core.Extensions;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;
using SoccerManager.Application.Queries.Player.GetPlayerProfile;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerProfile;

/// <summary>
/// Maps a player's league profile rows to <see cref="SquadPlayerProfileDto"/> instances.
/// </summary>
public static class GetLeaguePlayerProfileMapper
{
    /// <summary>
    /// Converts a registration's header row and the player's position rows into the profile's DTO representation.
    /// </summary>
    /// <param name="row">The header row returned by the stored procedure.</param>
    /// <param name="positions">The position rows returned by the stored procedure, the main position first.</param>
    /// <returns>The corresponding <see cref="SquadPlayerProfileDto"/>.</returns>
    public static SquadPlayerProfileDto ToDto(GetLeaguePlayerProfileResult row, IReadOnlyList<GetPlayerProfilePositionsResult> positions)
    {
        return new SquadPlayerProfileDto
        {
            Id = row.Id,
            PlayerId = row.PlayerId,
            LeagueTeamId = row.LeagueTeamId,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
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
            ContractLength = row.ContractLength,
            Morale = row.Morale,
            MoraleLabel = row.Morale.ToString().ToWords(),
            MoraleConcern = row.MoraleConcern,
            MoraleConcernLabel = row.MoraleConcern?.ToString().ToWords(),
            Condition = row.Condition,
            IsSuspended = row.IsSuspended,
            Positions = positions.Select(GetPlayerProfileMapper.ToPositionDto).ToList(),
        };
    }
}
