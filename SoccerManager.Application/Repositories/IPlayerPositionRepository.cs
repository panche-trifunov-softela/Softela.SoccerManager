using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="PlayerPosition"/> entities.
/// </summary>
public interface IPlayerPositionRepository
{
    /// <summary>
    /// Persists a new player position and returns its generated identifier.
    /// </summary>
    /// <param name="playerPosition">The player position to create.</param>
    /// <returns>The identifier assigned to the created player position.</returns>
    Task<int> CreateAsync(PlayerPosition playerPosition);

    /// <summary>
    /// Persists changes to an existing player position.
    /// </summary>
    /// <param name="playerPosition">The player position with updated values.</param>
    /// <returns>The identifier of the updated player position.</returns>
    Task<int> UpdateAsync(PlayerPosition playerPosition);

    /// <summary>
    /// Removes a player position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player position to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a player position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player position to retrieve.</param>
    /// <returns>The matching player position, or <see langword="null"/> when none exists.</returns>
    Task<PlayerPosition?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every position rating belonging to the given player.
    /// </summary>
    /// <param name="playerId">The identifier of the player whose position ratings to retrieve.</param>
    /// <returns>A list of every position rating belonging to the player.</returns>
    Task<List<PlayerPosition>> GetByPlayerIdAsync(int playerId);
}
