namespace SoccerManager.Application.Models;

/// <summary>
/// One row of the positions result set returned by the <c>dbo.GetPlayerProfile</c> and
/// <c>dbo.GetLeaguePlayerProfile</c> stored procedures: a position the player can play, with the spot it is drawn at on a
/// pitch. Kept separate from <see cref="Dtos.PlayerProfilePositionDto"/> because Dapper populates a result by setting
/// properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetPlayerProfilePositionsResult
{
    /// <summary>
    /// The identifier of the position.
    /// </summary>
    public int PositionId { get; set; }

    /// <summary>
    /// The name of the position.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The short name of the position, for example CB.
    /// </summary>
    public string ShortName { get; set; } = string.Empty;

    /// <summary>
    /// Whether this is the player's main (highest-quality) position.
    /// </summary>
    public bool IsMain { get; set; }

    /// <summary>
    /// The position's spot across the pitch, from 0 (the team's own left touchline) to 100, or <see langword="null"/> when
    /// no formation uses the position.
    /// </summary>
    public byte? PitchX { get; set; }

    /// <summary>
    /// The position's spot along the pitch, from 0 (the team's own goal line) to 100, or <see langword="null"/> when no
    /// formation uses the position.
    /// </summary>
    public byte? PitchY { get; set; }
}
