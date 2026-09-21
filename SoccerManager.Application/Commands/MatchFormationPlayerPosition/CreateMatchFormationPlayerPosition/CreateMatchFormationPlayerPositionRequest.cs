using MediatR;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.CreateMatchFormationPlayerPosition;

/// <summary>
/// Represents a request to create a new match formation player position. The player's condition, quality
/// and suspension status are not part of the request: the handler snapshots them from the league
/// registration and position rating.
/// </summary>
public sealed record CreateMatchFormationPlayerPositionRequest : IRequest<int>
{
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
