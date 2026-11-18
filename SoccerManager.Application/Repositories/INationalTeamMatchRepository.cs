using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="NationalTeamMatch"/> entities.
/// </summary>
public interface INationalTeamMatchRepository
{
    /// <summary>
    /// Persists a new national team match and returns its generated identifier.
    /// </summary>
    /// <param name="nationalTeamMatch">The national team match to create.</param>
    /// <returns>The identifier assigned to the created national team match.</returns>
    Task<int> CreateAsync(NationalTeamMatch nationalTeamMatch);

    /// <summary>
    /// Persists changes to an existing national team match.
    /// </summary>
    /// <param name="nationalTeamMatch">The national team match with updated values.</param>
    /// <returns>The identifier of the updated national team match.</returns>
    Task<int> UpdateAsync(NationalTeamMatch nationalTeamMatch);

    /// <summary>
    /// Removes a national team match by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team match to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a national team match by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team match to retrieve.</param>
    /// <returns>The matching national team match, or <see langword="null"/> when none exists.</returns>
    Task<NationalTeamMatch?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every national team match of a season, in kickoff order.
    /// </summary>
    /// <param name="seasonId">The identifier of the season whose national team matches to retrieve.</param>
    /// <returns>A list of every national team match of the season, in kickoff order.</returns>
    Task<List<NationalTeamMatch>> GetBySeasonIdAsync(int seasonId);
}
