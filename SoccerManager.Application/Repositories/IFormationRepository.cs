using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Formation"/> entities.
/// </summary>
public interface IFormationRepository
{
    /// <summary>
    /// Persists a new formation and returns its generated identifier.
    /// </summary>
    /// <param name="formation">The formation to create.</param>
    /// <returns>The identifier assigned to the created formation.</returns>
    Task<int> CreateAsync(Formation formation);

    /// <summary>
    /// Persists changes to an existing formation.
    /// </summary>
    /// <param name="formation">The formation with updated values.</param>
    /// <returns>The identifier of the updated formation.</returns>
    Task<int> UpdateAsync(Formation formation);

    /// <summary>
    /// Removes a formation by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a formation by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation to retrieve.</param>
    /// <returns>The matching formation, or <see langword="null"/> when none exists.</returns>
    Task<Formation?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all formations.
    /// </summary>
    /// <returns>A list of every formation.</returns>
    Task<List<Formation>> GetAllAsync();

    /// <summary>
    /// Retrieves a formation by its name.
    /// </summary>
    /// <param name="name">The name to match, compared case-insensitively by the database collation.</param>
    /// <returns>The matching formation, or <see langword="null"/> when none exists.</returns>
    Task<Formation?> GetByNameAsync(string name);

    /// <summary>
    /// Retrieves the formation a lineup defaults to: the one with the given name when it has all eleven starter slots,
    /// otherwise the first formation by name that does.
    /// </summary>
    /// <param name="preferredName">The name of the formation to prefer.</param>
    /// <returns>The chosen formation, or <see langword="null"/> when no formation has all eleven starter slots.</returns>
    Task<Formation?> GetDefaultForLineupAsync(string preferredName);
}
