using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="FormationPosition"/> entities.
/// </summary>
public interface IFormationPositionRepository
{
    /// <summary>
    /// Persists a new formation position and returns its generated identifier.
    /// </summary>
    /// <param name="formationPosition">The formation position to create.</param>
    /// <returns>The identifier assigned to the created formation position.</returns>
    Task<int> CreateAsync(FormationPosition formationPosition);

    /// <summary>
    /// Persists changes to an existing formation position.
    /// </summary>
    /// <param name="formationPosition">The formation position with updated values.</param>
    /// <returns>The identifier of the updated formation position.</returns>
    Task<int> UpdateAsync(FormationPosition formationPosition);

    /// <summary>
    /// Removes a formation position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation position to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a formation position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation position to retrieve.</param>
    /// <returns>The matching formation position, or <see langword="null"/> when none exists.</returns>
    Task<FormationPosition?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every position slot belonging to the given formation.
    /// </summary>
    /// <param name="formationId">The identifier of the formation whose position slots to retrieve.</param>
    /// <returns>A list of every position slot belonging to the formation.</returns>
    Task<List<FormationPosition>> GetByFormationIdAsync(int formationId);
}
