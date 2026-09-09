using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Player"/> entities.
/// </summary>
public interface IPlayerRepository
{
    /// <summary>
    /// Persists a new player and returns its generated identifier.
    /// </summary>
    /// <param name="player">The player to create.</param>
    /// <returns>The identifier assigned to the created player.</returns>
    Task<int> CreateAsync(Player player);

    /// <summary>
    /// Persists changes to an existing player.
    /// </summary>
    /// <param name="player">The player with updated values.</param>
    /// <returns>The identifier of the updated player.</returns>
    Task<int> UpdateAsync(Player player);

    /// <summary>
    /// Removes a player by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a player by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player to retrieve.</param>
    /// <returns>The matching player, or <see langword="null"/> when none exists.</returns>
    Task<Player?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all players.
    /// </summary>
    /// <returns>A list of every player.</returns>
    Task<List<Player>> GetAllAsync();
}
