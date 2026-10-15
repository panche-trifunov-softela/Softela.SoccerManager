namespace SoccerManager.Application.Models.SearchPlayers;

/// <summary>
/// One row returned by the <c>dbo.SearchPlayers</c> stored procedure: a player denormalised with the names of its
/// club, national team and main position. Kept separate from <see cref="Dtos.PlayerSearch.PlayerSearchItemDto"/> because Dapper
/// populates a result by setting properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class SearchPlayersResult
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
    /// The player's date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// The player's overall rating.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// The player's market value.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// The player's wage.
    /// </summary>
    public decimal Wage { get; set; }

    /// <summary>
    /// The URL of the player's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// The identifier of the player's current real-life club, or <see langword="null"/> when it has none.
    /// </summary>
    public int? TeamId { get; set; }

    /// <summary>
    /// The name of the player's current real-life club, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamName { get; set; }

    /// <summary>
    /// The identifier of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public int? NationalTeamId { get; set; }

    /// <summary>
    /// The name of the player's national team, or <see langword="null"/> when it has none.
    /// </summary>
    public string? NationalTeamName { get; set; }

    /// <summary>
    /// The identifier of the player's main (highest-quality) position, or <see langword="null"/> when it has none.
    /// </summary>
    public int? PositionId { get; set; }

    /// <summary>
    /// The name of the player's main position, or <see langword="null"/> when it has none.
    /// </summary>
    public string? PositionName { get; set; }
}
