using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Manager"/> entities.
/// </summary>
public interface IManagerRepository
{
    /// <summary>
    /// Persists a new manager and returns its generated identifier.
    /// </summary>
    /// <param name="manager">The manager to create.</param>
    /// <returns>The identifier assigned to the created manager.</returns>
    Task<int> CreateAsync(Manager manager);

    /// <summary>
    /// Persists changes to an existing manager.
    /// </summary>
    /// <param name="manager">The manager with updated values.</param>
    /// <returns>The identifier of the updated manager.</returns>
    Task<int> UpdateAsync(Manager manager);

    /// <summary>
    /// Removes a manager by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the manager to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a manager by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the manager to retrieve.</param>
    /// <returns>The matching manager, or <see langword="null"/> when none exists.</returns>
    Task<Manager?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves the manager profile linked to the given Keycloak user.
    /// </summary>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose manager profile is retrieved.</param>
    /// <returns>The matching manager, or <see langword="null"/> when the user has no manager profile.</returns>
    Task<Manager?> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves all managers.
    /// </summary>
    /// <returns>A list of every manager.</returns>
    Task<List<Manager>> GetAllAsync();
}
