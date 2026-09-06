using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Season"/> entities.
/// </summary>
public interface ISeasonRepository
{
    /// <summary>
    /// Persists a new season and returns its generated identifier.
    /// </summary>
    /// <param name="season">The season to create.</param>
    /// <returns>The identifier assigned to the created season.</returns>
    Task<int> CreateAsync(Season season);

    /// <summary>
    /// Persists changes to an existing season.
    /// </summary>
    /// <param name="season">The season with updated values.</param>
    /// <returns>The identifier of the updated season.</returns>
    Task<int> UpdateAsync(Season season);

    /// <summary>
    /// Removes a season by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the season to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a season by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the season to retrieve.</param>
    /// <returns>The matching season, or <see langword="null"/> when none exists.</returns>
    Task<Season?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every season belonging to the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose seasons to retrieve.</param>
    /// <returns>A list of every season belonging to the league.</returns>
    Task<List<Season>> GetByLeagueIdAsync(int leagueId);
}
