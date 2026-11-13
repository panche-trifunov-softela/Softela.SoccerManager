using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeamPlayerInjury"/> entities.
/// </summary>
public interface ILeagueTeamPlayerInjuryRepository
{
    /// <summary>
    /// Persists a new injury and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerInjury">The injury to create.</param>
    /// <returns>The identifier assigned to the created injury.</returns>
    Task<int> CreateAsync(LeagueTeamPlayerInjury leagueTeamPlayerInjury);

    /// <summary>
    /// Persists changes to an existing injury.
    /// </summary>
    /// <param name="leagueTeamPlayerInjury">The injury with updated values.</param>
    /// <returns>The identifier of the updated injury.</returns>
    Task<int> UpdateAsync(LeagueTeamPlayerInjury leagueTeamPlayerInjury);

    /// <summary>
    /// Removes an injury by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the injury to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves an injury by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the injury to retrieve.</param>
    /// <returns>The matching injury, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeamPlayerInjury?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every injury of a squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row whose injuries to retrieve.</param>
    /// <returns>A list of every injury of the squad row.</returns>
    Task<List<LeagueTeamPlayerInjury>> GetByLeagueTeamPlayerIdAsync(int leagueTeamPlayerId);
}
