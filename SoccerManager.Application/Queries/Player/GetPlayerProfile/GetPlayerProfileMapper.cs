using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.Player.GetPlayerProfile;

/// <summary>
/// Maps a player's profile rows to <see cref="PlayerProfileDto"/> instances.
/// </summary>
public static class GetPlayerProfileMapper
{
    /// <summary>
    /// Converts a player's header row and position rows into the profile's DTO representation.
    /// </summary>
    /// <param name="row">The header row returned by the stored procedure.</param>
    /// <param name="positions">The position rows returned by the stored procedure, the main position first.</param>
    /// <returns>The corresponding <see cref="PlayerProfileDto"/>.</returns>
    public static PlayerProfileDto ToDto(GetPlayerProfileResult row, IReadOnlyList<GetPlayerProfilePositionsResult> positions)
    {
        return new PlayerProfileDto
        {
            Id = row.Id,
            Name = row.Name,
            ImageUrl = row.ImageUrl,
            DateOfBirth = row.DateOfBirth,
            TeamId = row.TeamId,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            NationalTeamId = row.NationalTeamId,
            NationalTeamName = row.NationalTeamName,
            NationalTeamLogoUrl = row.NationalTeamLogoUrl,
            PositionId = row.PositionId,
            PositionName = row.PositionName,
            PositionShortName = row.PositionShortName,
            Rating = row.Rating,
            Value = row.Value,
            Wage = row.Wage,
            Positions = positions.Select(ToPositionDto).ToList(),
        };
    }

    /// <summary>
    /// Converts a position row into its DTO representation. The game world profile maps its positions through this too.
    /// </summary>
    /// <param name="row">The position row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="PlayerProfilePositionDto"/>.</returns>
    public static PlayerProfilePositionDto ToPositionDto(GetPlayerProfilePositionsResult row)
    {
        return new PlayerProfilePositionDto
        {
            PositionId = row.PositionId,
            Name = row.Name,
            ShortName = row.ShortName,
            IsMain = row.IsMain,
            PitchX = row.PitchX,
            PitchY = row.PitchY,
        };
    }
}
