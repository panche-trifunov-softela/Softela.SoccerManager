namespace SoccerManager.Application.Models;

/// <summary>
/// The header row returned by the <c>dbo.GetPlayerProfile</c> stored procedure: a player with the real-life club, national
/// team, main position and current rating. Kept separate from <see cref="Dtos.PlayerProfileDto"/> because Dapper populates a
/// result by setting properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetPlayerProfileResult
{
    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int Id { get; set; }

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
    /// The identifier of the player's current real-life club, or <see langword="null"/> when it has none.
    /// </summary>
    public int? TeamId { get; set; }

    /// <summary>
    /// The name of the player's current real-life club, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamName { get; set; }

    /// <summary>
    /// The URL of the club's logo, or <see langword="null"/> when there is no club or no logo.
    /// </summary>
    public string? TeamLogoUrl { get; set; }

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
    /// The identifier of the player's main (highest-quality) position, or 0 when the player has no position.
    /// </summary>
    public int PositionId { get; set; }

    /// <summary>
    /// The name of the player's main position, or empty when the player has no position.
    /// </summary>
    public string PositionName { get; set; } = string.Empty;

    /// <summary>
    /// The short name of the player's main position, for example CB, or empty when the player has no position.
    /// </summary>
    public string PositionShortName { get; set; } = string.Empty;

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// The player's market value.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// The player's wage.
    /// </summary>
    public decimal Wage { get; set; }
}
