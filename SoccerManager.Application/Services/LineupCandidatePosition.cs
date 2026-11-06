using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Services;

/// <summary>
/// Represents one position rating of a squad player, as the lineup logic reads it.
/// </summary>
public sealed record LineupCandidatePosition
{
    /// <summary>
    /// The identifier of the player position rating.
    /// </summary>
    public int PlayerPositionId { get; init; }

    /// <summary>
    /// The identifier of the position the rating is for.
    /// </summary>
    public int PositionId { get; init; }

    /// <summary>
    /// The quality of the player at the position, from 1 to 100.
    /// </summary>
    public int Quality { get; init; }

    /// <summary>
    /// The area of the pitch the position occupies.
    /// </summary>
    public PositionArea Area { get; init; }

    /// <summary>
    /// Whether the position is in the goalkeeper area.
    /// </summary>
    public bool IsGoalkeeper => Area == PositionArea.Goalkeeper;
}
