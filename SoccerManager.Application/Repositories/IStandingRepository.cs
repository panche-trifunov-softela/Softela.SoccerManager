using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Standing"/> entities.
/// </summary>
public interface IStandingRepository
{
    /// <summary>
    /// Persists a new standing and returns its generated identifier.
    /// </summary>
    /// <param name="standing">The standing to create.</param>
    /// <returns>The identifier assigned to the created standing.</returns>
    Task<int> CreateAsync(Standing standing);

    /// <summary>
    /// Persists changes to an existing standing.
    /// </summary>
    /// <param name="standing">The standing with updated values.</param>
    /// <returns>The identifier of the updated standing.</returns>
    Task<int> UpdateAsync(Standing standing);

    /// <summary>
    /// Removes a standing by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the standing to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a standing by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the standing to retrieve.</param>
    /// <returns>The matching standing, or <see langword="null"/> when none exists.</returns>
    Task<Standing?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every standing belonging to the given competition and season.
    /// </summary>
    /// <param name="competitionId">The identifier of the competition to filter by.</param>
    /// <param name="seasonId">The identifier of the season to filter by.</param>
    /// <returns>A list of every standing belonging to the competition and season.</returns>
    Task<List<Standing>> GetByCompetitionAndSeasonAsync(int competitionId, int seasonId);
}
