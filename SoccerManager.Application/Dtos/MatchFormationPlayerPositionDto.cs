namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a match formation player position for read-oriented consumers.
/// </summary>
public sealed record MatchFormationPlayerPositionDto
{
    /// <summary>
    /// The identifier of the match formation player position.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the match the lineup is for.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the team lining up.
    /// </summary>
    public int TeamId { get; init; }

    /// <summary>
    /// The identifier of the formation slot being filled.
    /// </summary>
    public int FormationPositionId { get; init; }

    /// <summary>
    /// The identifier of the player position rating filling the slot.
    /// </summary>
    public int PlayerPositionId { get; init; }

    /// <summary>
    /// The player's condition at the time of the match, from 1 to 100, copied from the league
    /// registration.
    /// </summary>
    public int ConditionOnMatch { get; init; }

    /// <summary>
    /// The player's quality in the position at the time of the match, from 1 to 100, copied from
    /// the position rating.
    /// </summary>
    public int QualityAtPositionOnMatch { get; init; }

    /// <summary>
    /// Whether the player is suspended for the match, copied from the league registration.
    /// </summary>
    public bool IsSuspended { get; init; }

    /// <summary>
    /// The UTC date and time the match formation player position was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the match formation player position was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
