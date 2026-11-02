namespace SoccerManager.Application.Services;

/// <summary>
/// Represents a registered squad player with what picking a lineup needs.
/// </summary>
public sealed record LineupCandidate
{
    /// <summary>
    /// The identifier of the player.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The player's squad number.
    /// </summary>
    public int SquadNumber { get; init; }

    /// <summary>
    /// The player's physical condition, from 1 to 100.
    /// </summary>
    public int Condition { get; init; }

    /// <summary>
    /// Whether the player is suspended in the domestic competition.
    /// </summary>
    public bool IsSuspended { get; init; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; init; }

    /// <summary>
    /// The player's position ratings, best first: highest quality, then lowest player position identifier. Empty when the
    /// player has no position data.
    /// </summary>
    public IReadOnlyList<LineupCandidatePosition> Positions { get; init; } = [];

    /// <summary>
    /// Whether the player has at least one position rating.
    /// </summary>
    public bool HasPositionData => Positions.Count > 0;

    /// <summary>
    /// Whether every position rating the player has is a goalkeeper one.
    /// </summary>
    public bool OnlyGoalkeeper => Positions.Count > 0 && Positions.All(position => position.IsGoalkeeper);

    /// <summary>
    /// The player's best position rating, or <see langword="null"/> when the player has none.
    /// </summary>
    public LineupCandidatePosition? Main => Positions.Count > 0 ? Positions[0] : null;

    /// <summary>
    /// Finds the player's best rating for the given position.
    /// </summary>
    /// <param name="positionId">The identifier of the position, or <see langword="null"/> for a slot that asks for none.</param>
    /// <returns>The rating for the position, or <see langword="null"/> when the player has none for it.</returns>
    public LineupCandidatePosition? RowFor(int? positionId) =>
        positionId is null ? null : Positions.FirstOrDefault(position => position.PositionId == positionId);
}
