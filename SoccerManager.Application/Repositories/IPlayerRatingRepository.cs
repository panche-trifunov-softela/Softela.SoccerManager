using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="PlayerRating"/> entities.
/// </summary>
public interface IPlayerRatingRepository
{
    /// <summary>
    /// Makes a new rating the player's current one, keeping the previous current rating as history, and returns its identifier.
    /// </summary>
    /// <param name="playerRating">The player rating to create.</param>
    /// <returns>The identifier assigned to the created player rating.</returns>
    Task<int> CreateAsync(PlayerRating playerRating);

    /// <summary>
    /// Corrects the value of an existing rating.
    /// </summary>
    /// <param name="playerRating">The player rating with the corrected value.</param>
    /// <returns>The identifier of the updated player rating.</returns>
    Task<int> UpdateAsync(PlayerRating playerRating);

    /// <summary>
    /// Removes a player rating by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player rating to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a player rating by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player rating to retrieve.</param>
    /// <returns>The matching player rating, or <see langword="null"/> when none exists.</returns>
    Task<PlayerRating?> GetByIdAsync(int id);

    /// <summary>
    /// Gets every rating the player has had, newest first.
    /// </summary>
    /// <param name="playerId">The identifier of the player whose ratings to retrieve.</param>
    /// <returns>A list of every rating the player has had, newest first.</returns>
    Task<List<PlayerRating>> GetByPlayerIdAsync(int playerId);

    /// <summary>
    /// Gets the player's rating in force, or null when there is no such player.
    /// </summary>
    /// <param name="playerId">The identifier of the player whose current rating to retrieve.</param>
    /// <returns>The player's current rating, or <see langword="null"/> when the player does not exist.</returns>
    Task<PlayerRating?> GetCurrentByPlayerIdAsync(int playerId);
}
