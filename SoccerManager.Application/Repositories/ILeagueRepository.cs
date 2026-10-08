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
    /// Retrieves the newest leagues in which the given user does not currently manage a club.
    /// </summary>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose currently managed leagues are left out.</param>
    /// <param name="searchTerm">The text a league name must contain, or <see langword="null"/> for no name filter.</param>
    /// <param name="maxCount">The most leagues to return.</param>
    /// <returns>The matching leagues, newest first.</returns>
    Task<List<League>> GetAvailableForUserAsync(Guid userId, string? searchTerm, int maxCount);
}
