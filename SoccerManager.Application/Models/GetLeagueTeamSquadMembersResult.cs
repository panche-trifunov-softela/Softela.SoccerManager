using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetLeagueTeamSquadMembers</c> stored procedure: an entry on a league team's squad,
/// denormalised with the player's details, current rating, main position and national team. Kept separate from
/// <see cref="Dtos.LeagueTeamSquadMemberDto"/> because Dapper populates a result by setting properties, and the Dto is an
/// init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeagueTeamSquadMembersResult
{
    /// <summary>
    /// The identifier of the squad entry (the league team player).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// The name of the player.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// The player's date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// The player's squad number, from 1 to 99 inclusive.
    /// </summary>
    public int SquadNumber { get; set; }

    /// <summary>
    /// The identifier of the player's main (highest-quality) position.
    /// </summary>
    public int PositionId { get; set; }

    /// <summary>
    /// The name of the player's main position.
    /// </summary>
    public string PositionName { get; set; } = string.Empty;

    /// <summary>
    /// The short name of the player's main position, for example CB.
    /// </summary>
    public string PositionShortName { get; set; } = string.Empty;

    /// <summary>
    /// The identifier of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public int? NationalTeamId { get; set; }

    /// <summary>
    /// The name of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public string? NationalTeamName { get; set; }

    /// <summary>
    /// The URL of the national team's logo, or <see langword="null"/> when there is no national team or no logo.
    /// </summary>
    public string? NationalTeamLogoUrl { get; set; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// The player's value on the transfer market.
    /// </summary>
    public long Value { get; set; }

    /// <summary>
    /// The weekly salary the player's contract pays.
    /// </summary>
    public long Wage { get; set; }

    /// <summary>
    /// The player's morale.
    /// </summary>
    public Morale Morale { get; set; }

    /// <summary>
    /// The player's physical condition, from 1 to 100 inclusive.
    /// </summary>
    public int Condition { get; set; }
}
