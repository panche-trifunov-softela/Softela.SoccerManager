using MediatR;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.UpdateMatchFormationPlayerPosition;

/// <summary>
/// Represents a request to update an existing match formation player position. The player's condition,
/// quality and suspension status are not part of the request: the handler snapshots them from the league
/// registration and position rating.
/// </summary>
public sealed record UpdateMatchFormationPlayerPositionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match formation player position to update.
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
}
