using FluentValidation;

namespace SoccerManager.Application.Services;

/// <summary>
/// Holds the rules an absence's create and update share, so both are checked by one definition: the club plays the match,
/// the player is in the club's squad, the player has no other absence in the match, and the player did not play in it.
/// </summary>
public interface IMatchPlayerAbsenceService
{
    /// <summary>
    /// Ensures a player's absence from a match is allowed. It checks, in this order, that the club plays the match, that the
    /// player is in the club's squad, that the player has no other absence in the match, and that the player played no
    /// minutes in it.
    /// </summary>
    /// <param name="matchId">The identifier of the match the player misses.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) the player is missing from.</param>
    /// <param name="playerId">The identifier of the player who is absent.</param>
    /// <param name="ownId">The identifier of the absence being updated, so it does not count as a second absence; <see langword="null"/> when an absence is created.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match, the player is not in the club's squad, the player already has an absence in the match, or the player played in it.</exception>
    Task EnsureAbsenceIsAllowedAsync(int matchId, int leagueTeamId, int playerId, int? ownId);
}
