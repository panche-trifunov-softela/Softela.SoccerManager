using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents an entry on a league team's squad: the player with their date of birth, current rating, main position and
/// national team, and the entry's squad number, value, weekly wage, morale and condition.
/// </summary>
public sealed record LeagueTeamSquadMemberDto
{
    /// <summary>
    /// The identifier of the squad entry (the league team player).
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The name of the player.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The player's date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; init; }

    /// <summary>
    /// The player's squad number, from 1 to 99 inclusive.
    /// </summary>
    public int SquadNumber { get; init; }

    /// <summary>
    /// The identifier of the player's main (highest-quality) position.
    /// </summary>
    public int PositionId { get; init; }

    /// <summary>
    /// The name of the player's main position.
    /// </summary>
    public required string PositionName { get; init; }

    /// <summary>
    /// The short name of the player's main position, for example CB.
    /// </summary>
    public required string PositionShortName { get; init; }

    /// <summary>
    /// The identifier of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public int? NationalTeamId { get; init; }

    /// <summary>
    /// The name of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public string? NationalTeamName { get; init; }

    /// <summary>
    /// The URL of the national team's logo, or <see langword="null"/> when there is no national team or no logo.
    /// </summary>
    public string? NationalTeamLogoUrl { get; init; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; init; }

    /// <summary>
    /// The player's value on the transfer market.
    /// </summary>
    public long Value { get; init; }

    /// <summary>
    /// The weekly salary the player's contract pays.
    /// </summary>
    public long Wage { get; init; }

    /// <summary>
    /// The player's morale.
    /// </summary>
    public Morale Morale { get; init; }

    /// <summary>
    /// The player's physical condition, from 1 to 100 inclusive.
    /// </summary>
    public int Condition { get; init; }
}
