using SoccerManager.Application.Models;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="MatchFormationPlayerPosition"/> entities.
/// </summary>
public interface IMatchFormationPlayerPositionRepository
{
    /// <summary>
    /// Persists a new match formation player position and returns its generated identifier.
    /// </summary>
    /// <param name="matchFormationPlayerPosition">The match formation player position to create.</param>
    /// <returns>The identifier assigned to the created match formation player position.</returns>
    Task<int> CreateAsync(MatchFormationPlayerPosition matchFormationPlayerPosition);

    /// <summary>
    /// Persists changes to an existing match formation player position.
    /// </summary>
    /// <param name="matchFormationPlayerPosition">The match formation player position with updated values.</param>
    /// <returns>The identifier of the updated match formation player position.</returns>
    Task<int> UpdateAsync(MatchFormationPlayerPosition matchFormationPlayerPosition);

    /// <summary>
    /// Removes a match formation player position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match formation player position to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Removes a player's lineup slots for a club in matches that have not started. Slots in started matches
    /// are kept as the record of who played.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) whose slots to remove.</param>
    /// <param name="playerId">The identifier of the player whose slots to remove.</param>
    Task DeleteUnstartedByLeagueTeamAndPlayerAsync(int leagueTeamId, int playerId);

    /// <summary>
    /// Retrieves a match formation player position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match formation player position to retrieve.</param>
    /// <returns>The matching match formation player position, or <see langword="null"/> when none exists.</returns>
    Task<MatchFormationPlayerPosition?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every lineup slot recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose lineup slots to retrieve.</param>
    /// <returns>A list of every lineup slot recorded for the match.</returns>
    Task<List<MatchFormationPlayerPosition>> GetByMatchIdAsync(int matchId);

    /// <summary>
    /// Retrieves a club's lineup rows for a match, each joined to its formation slot and to the player behind its position
    /// rating, in slot order. The rows can span several formations.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose lineup to retrieve.</param>
    /// <returns>A list of the club's lineup rows, empty when it has none.</returns>
    Task<List<GetMatchLineupRowsResult>> GetLineupRowsAsync(int matchId, int leagueTeamId);

    /// <summary>
    /// Removes a club's whole lineup for a match.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose lineup to remove.</param>
    Task DeleteByMatchAndLeagueTeamAsync(int matchId, int leagueTeamId);
}
