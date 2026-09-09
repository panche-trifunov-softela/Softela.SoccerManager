using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Team"/> entities.
/// </summary>
public interface ITeamRepository
{
    /// <summary>
    /// Persists a new team and returns its generated identifier.
    /// </summary>
    /// <param name="team">The team to create.</param>
    /// <returns>The identifier assigned to the created team.</returns>
    Task<int> CreateAsync(Team team);

    /// <summary>
    /// Persists changes to an existing team.
    /// </summary>
    /// <param name="team">The team with updated values.</param>
    /// <returns>The identifier of the updated team.</returns>
    Task<int> UpdateAsync(Team team);

    /// <summary>
    /// Removes a team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the team to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the team to retrieve.</param>
    /// <returns>The matching team, or <see langword="null"/> when none exists.</returns>
    Task<Team?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all teams.
    /// </summary>
    /// <returns>A list of every team.</returns>
    Task<List<Team>> GetAllAsync();
}
