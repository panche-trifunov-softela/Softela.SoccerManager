using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="League"/> entities.
/// </summary>
public interface ILeagueRepository
{
    /// <summary>
    /// Persists a new league and returns its generated identifier.
    /// </summary>
    /// <param name="league">The league to create.</param>
    /// <returns>The identifier assigned to the created league.</returns>
    Task<int> CreateAsync(League league);

    /// <summary>
    /// Persists changes to an existing league.
    /// </summary>
    /// <param name="league">The league with updated values.</param>
    /// <returns>The identifier of the updated league.</returns>
    Task<int> UpdateAsync(League league);

    /// <summary>
    /// Removes a league by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a league by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league to retrieve.</param>
    /// <returns>The matching league, or <see langword="null"/> when none exists.</returns>
    Task<League?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all leagues.
    /// </summary>
    /// <returns>A list of every league.</returns>
    Task<List<League>> GetAllAsync();
}
