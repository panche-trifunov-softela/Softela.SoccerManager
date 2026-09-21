using FluentValidation;

namespace SoccerManager.Application.Services;

/// <summary>
/// Resolves the values a match formation player position copies from other aggregates, so the create and
/// update handlers share one definition of where each value comes from.
/// </summary>
public interface IMatchFormationPlayerPositionSnapshotResolver
{
    /// <summary>
    /// Loads the player's league registration and position rating for the given match and team and returns
    /// the values to snapshot.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="teamId">The identifier of the team the player is lining up for.</param>
    /// <param name="playerPositionId">The identifier of the player position rating filling the slot.</param>
    /// <returns>The values to copy onto the match formation player position.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match, its season, the player position or the player's registration in the match's league does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the player is registered with a different team in the match's league.</exception>
    Task<MatchFormationPlayerPositionSnapshot> ResolveAsync(int matchId, int teamId, int playerPositionId);
}
