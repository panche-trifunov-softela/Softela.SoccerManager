using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Stadium"/> entities.
/// </summary>
public interface IStadiumRepository
{
    /// <summary>
    /// Persists a new stadium and returns its generated identifier.
    /// </summary>
    /// <param name="stadium">The stadium to create.</param>
    /// <returns>The identifier assigned to the created stadium.</returns>
    Task<int> CreateAsync(Stadium stadium);

    /// <summary>
    /// Persists changes to an existing stadium.
    /// </summary>
    /// <param name="stadium">The stadium with updated values.</param>
    /// <returns>The identifier of the updated stadium.</returns>
    Task<int> UpdateAsync(Stadium stadium);

    /// <summary>
    /// Removes a stadium by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the stadium to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a stadium by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the stadium to retrieve.</param>
    /// <returns>The matching stadium, or <see langword="null"/> when none exists.</returns>
    Task<Stadium?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all stadiums.
    /// </summary>
    /// <returns>A list of every stadium.</returns>
    Task<List<Stadium>> GetAllAsync();
}
