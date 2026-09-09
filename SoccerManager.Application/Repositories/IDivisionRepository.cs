using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Division"/> entities.
/// </summary>
public interface IDivisionRepository
{
    /// <summary>
    /// Persists a new division and returns its generated identifier.
    /// </summary>
    /// <param name="division">The division to create.</param>
    /// <returns>The identifier assigned to the created division.</returns>
    Task<int> CreateAsync(Division division);

    /// <summary>
    /// Persists changes to an existing division.
    /// </summary>
    /// <param name="division">The division with updated values.</param>
    /// <returns>The identifier of the updated division.</returns>
    Task<int> UpdateAsync(Division division);

    /// <summary>
    /// Removes a division by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the division to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a division by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the division to retrieve.</param>
    /// <returns>The matching division, or <see langword="null"/> when none exists.</returns>
    Task<Division?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every division belonging to the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose divisions to retrieve.</param>
    /// <returns>A list of every division belonging to the league.</returns>
    Task<List<Division>> GetByLeagueIdAsync(int leagueId);
}
