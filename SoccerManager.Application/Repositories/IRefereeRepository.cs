using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Referee"/> entities.
/// </summary>
public interface IRefereeRepository
{
    /// <summary>
    /// Persists a new referee and returns its generated identifier.
    /// </summary>
    /// <param name="referee">The referee to create.</param>
    /// <returns>The identifier assigned to the created referee.</returns>
    Task<int> CreateAsync(Referee referee);

    /// <summary>
    /// Persists changes to an existing referee.
    /// </summary>
    /// <param name="referee">The referee with updated values.</param>
    /// <returns>The identifier of the updated referee.</returns>
    Task<int> UpdateAsync(Referee referee);

    /// <summary>
    /// Removes a referee by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the referee to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a referee by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the referee to retrieve.</param>
    /// <returns>The matching referee, or <see langword="null"/> when none exists.</returns>
    Task<Referee?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all referees.
    /// </summary>
    /// <returns>A list of every referee.</returns>
    Task<List<Referee>> GetAllAsync();
}
