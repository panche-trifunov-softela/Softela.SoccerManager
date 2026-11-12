using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a player's profile in a game world: the player's registration at a club of the league, with the player's
/// date of birth, current rating, main position and national team, and the registration's squad number, value, weekly wage,
/// contract, morale, condition and suspension, and every position the player can play.
/// </summary>
public sealed record SquadPlayerProfileDto
{
    /// <summary>
    /// The identifier of the registration (the league team player).
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The identifier of the league team the player is registered at: the requested club or, failing that, a rival club in
    /// the same league.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The name of the club the player is registered at.
    /// </summary>
    public required string TeamName { get; init; }

    /// <summary>
    /// The URL of that club's logo, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; init; }

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
    /// The number of seasons left on the player's contract.
    /// </summary>
    public int ContractLength { get; init; }

    /// <summary>
    /// The player's morale.
    /// </summary>
    public Morale Morale { get; init; }

    /// <summary>
    /// The player's morale in words, for example Very satisfied.
    /// </summary>
    public required string MoraleLabel { get; init; }

    /// <summary>
    /// The reason behind the player's morale, or <see langword="null"/> when there is none.
    /// </summary>
    public MoraleConcern? MoraleConcern { get; init; }

    /// <summary>
    /// The reason behind the player's morale in words, for example Playing time, or <see langword="null"/> when there is
    /// none.
    /// </summary>
    public string? MoraleConcernLabel { get; init; }

    /// <summary>
    /// The player's physical condition, from 1 to 100 inclusive.
    /// </summary>
    public int Condition { get; init; }

    /// <summary>
    /// Whether the player is suspended in the domestic competition, and so can't be picked for a lineup.
    /// </summary>
    public bool IsSuspended { get; init; }

    /// <summary>
    /// Every position the player can play, the main position first.
    /// </summary>
    public required IReadOnlyList<PlayerProfilePositionDto> Positions { get; init; }
}
