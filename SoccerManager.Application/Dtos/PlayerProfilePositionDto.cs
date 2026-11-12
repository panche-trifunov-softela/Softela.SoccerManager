namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a position a player can play, with the spot it is drawn at on a pitch, for the player profile.
/// </summary>
public sealed record PlayerProfilePositionDto
{
    /// <summary>
    /// The identifier of the position.
    /// </summary>
    public int PositionId { get; init; }

    /// <summary>
    /// The name of the position.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The short name of the position, for example CB.
    /// </summary>
    public required string ShortName { get; init; }

    /// <summary>
    /// Whether this is the player's main (highest-quality) position.
    /// </summary>
    public bool IsMain { get; init; }

    /// <summary>
    /// The position's spot across the pitch, from 0 (the team's own left touchline) to 100, or <see langword="null"/> when
    /// no formation uses the position.
    /// </summary>
    public byte? PitchX { get; init; }

    /// <summary>
    /// The position's spot along the pitch, from 0 (the team's own goal line) to 100, or <see langword="null"/> when no
    /// formation uses the position.
    /// </summary>
    public byte? PitchY { get; init; }
}
