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
}
