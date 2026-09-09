using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Position"/> entities.
/// </summary>
public interface IPositionRepository
{
    /// <summary>
    /// Persists a new position and returns its generated identifier.
    /// </summary>
    /// <param name="position">The position to create.</param>
    /// <returns>The identifier assigned to the created position.</returns>
    Task<int> CreateAsync(Position position);

    /// <summary>
    /// Persists changes to an existing position.
    /// </summary>
    /// <param name="position">The position with updated values.</param>
    /// <returns>The identifier of the updated position.</returns>
    Task<int> UpdateAsync(Position position);

    /// <summary>
    /// Removes a position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the position to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the position to retrieve.</param>
    /// <returns>The matching position, or <see langword="null"/> when none exists.</returns>
    Task<Position?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all positions.
    /// </summary>
    /// <returns>A list of every position.</returns>
    Task<List<Position>> GetAllAsync();
}
