using SoccerManager.Application.Models;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="NationalTeam"/> entities.
/// </summary>
public interface INationalTeamRepository
{
    /// <summary>
    /// Persists a new national team and returns its generated identifier.
    /// </summary>
    /// <param name="nationalTeam">The national team to create.</param>
    /// <returns>The identifier assigned to the created national team.</returns>
    Task<int> CreateAsync(NationalTeam nationalTeam);

    /// <summary>
    /// Persists changes to an existing national team.
    /// </summary>
    /// <param name="nationalTeam">The national team with updated values.</param>
    /// <returns>The identifier of the updated national team.</returns>
    Task<int> UpdateAsync(NationalTeam nationalTeam);

    /// <summary>
    /// Removes a national team by its identifier, but only when no league still has standings, match rows or deals for it.
    /// The check and the delete are atomic inside the stored procedure.
    /// </summary>
    /// <param name="id">The identifier of the national team to delete.</param>
    /// <returns>
    /// Whether the national team was removed and, when it was not, which kinds of record still block it.
    /// When nothing is flagged as blocking, the national team did not exist.
    /// </returns>
    Task<DeleteNationalTeamResult> DeleteAsync(int id);

    /// <summary>
    /// Retrieves a national team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team to retrieve.</param>
    /// <returns>The matching national team, or <see langword="null"/> when none exists.</returns>
    Task<NationalTeam?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all national teams.
    /// </summary>
    /// <returns>A list of every national team.</returns>
    Task<List<NationalTeam>> GetAllAsync();
}
