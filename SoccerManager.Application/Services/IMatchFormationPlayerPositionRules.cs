using FluentValidation;

namespace SoccerManager.Application.Services;

/// <summary>
/// Enforces the rules a lineup slot must obey for every caller, so the create and update handlers share one
/// definition of who may line up.
/// </summary>
public interface IMatchFormationPlayerPositionRules
{
    /// <summary>
    /// Ensures the player filling the slot is not suspended and has no other slot in the club's lineup for the match.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) the player is lining up for.</param>
    /// <param name="playerPositionId">The identifier of the player position rating filling the slot.</param>
    /// <param name="isSuspended">Whether the player is suspended for the match, as resolved for the slot's snapshot.</param>
    /// <param name="excludedSlotId">The identifier of the slot being updated, which does not count as another slot, or <see langword="null"/> for a new slot.</param>
    /// <exception cref="KeyNotFoundException">Thrown when a player position involved does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the player is suspended or already has a slot in the lineup.</exception>
    Task EnsureValidAsync(int matchId, int leagueTeamId, int playerPositionId, bool isSuspended, int? excludedSlotId);
}
