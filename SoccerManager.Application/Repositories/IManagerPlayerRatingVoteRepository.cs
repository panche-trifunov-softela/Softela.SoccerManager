using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="ManagerPlayerRatingVote"/> entities.
/// </summary>
public interface IManagerPlayerRatingVoteRepository
{
    /// <summary>
    /// Persists a new manager player rating vote and returns its generated identifier.
    /// </summary>
    /// <param name="managerPlayerRatingVote">The vote to create.</param>
    /// <returns>The identifier assigned to the created vote.</returns>
    Task<int> CreateAsync(ManagerPlayerRatingVote managerPlayerRatingVote);

    /// <summary>
    /// Updates the direction of an existing vote.
    /// </summary>
    /// <param name="managerPlayerRatingVote">The vote with its updated direction.</param>
    /// <returns>The identifier of the updated vote.</returns>
    Task<int> UpdateAsync(ManagerPlayerRatingVote managerPlayerRatingVote);

    /// <summary>
    /// Removes a vote by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the vote to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a vote by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the vote to retrieve.</param>
    /// <returns>The matching vote, or <see langword="null"/> when none exists.</returns>
    Task<ManagerPlayerRatingVote?> GetByIdAsync(int id);

    /// <summary>
    /// Gets every vote on a player, newest first.
    /// </summary>
    /// <param name="playerId">The identifier of the player whose votes to retrieve.</param>
    /// <returns>A list of every vote on the player, newest first.</returns>
    Task<List<ManagerPlayerRatingVote>> GetByPlayerIdAsync(int playerId);

    /// <summary>
    /// Gets a manager's vote on a player at one rating, if they cast one.
    /// </summary>
    /// <param name="managerId">The identifier of the manager.</param>
    /// <param name="playerId">The identifier of the player.</param>
    /// <param name="ratingAtTheMoment">The player's rating the vote was cast against.</param>
    /// <returns>The matching vote, or <see langword="null"/> when the manager has not voted at that rating.</returns>
    Task<ManagerPlayerRatingVote?> GetByManagerPlayerAndRatingAsync(int managerId, int playerId, int ratingAtTheMoment);
}
