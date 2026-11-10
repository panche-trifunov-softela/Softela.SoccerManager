using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="PenaltyAfterExtraTime"/> entities.
/// </summary>
public interface IPenaltyAfterExtraTimeRepository
{
    /// <summary>
    /// Persists a new penalty after extra time and returns its generated identifier.
    /// </summary>
    /// <param name="penaltyAfterExtraTime">The penalty after extra time to create.</param>
    /// <returns>The identifier assigned to the created penalty after extra time.</returns>
    Task<int> CreateAsync(PenaltyAfterExtraTime penaltyAfterExtraTime);

    /// <summary>
    /// Persists changes to an existing penalty after extra time.
    /// </summary>
    /// <param name="penaltyAfterExtraTime">The penalty after extra time with updated values.</param>
    /// <returns>The identifier of the updated penalty after extra time.</returns>
    Task<int> UpdateAsync(PenaltyAfterExtraTime penaltyAfterExtraTime);

    /// <summary>
    /// Removes a penalty after extra time by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the penalty after extra time to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a penalty after extra time by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the penalty after extra time to retrieve.</param>
    /// <returns>The matching penalty after extra time, or <see langword="null"/> when none exists.</returns>
    Task<PenaltyAfterExtraTime?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every kick of the shootout of a match, in kick order.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose shootout kicks to retrieve.</param>
    /// <returns>A list of every kick recorded for the match, in kick order.</returns>
    Task<List<PenaltyAfterExtraTime>> GetByMatchIdAsync(int matchId);
}
