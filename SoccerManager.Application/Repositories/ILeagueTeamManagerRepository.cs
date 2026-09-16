using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeamManager"/> entities.
/// </summary>
public interface ILeagueTeamManagerRepository
{
    /// <summary>
    /// Persists a new league team manager and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamManager">The league team manager to create.</param>
    /// <returns>The identifier assigned to the created league team manager.</returns>
    Task<int> CreateAsync(LeagueTeamManager leagueTeamManager);

    /// <summary>
    /// Persists changes to an existing league team manager.
    /// </summary>
    /// <param name="leagueTeamManager">The league team manager with updated values.</param>
    /// <returns>The identifier of the updated league team manager.</returns>
    Task<int> UpdateAsync(LeagueTeamManager leagueTeamManager);

    /// <summary>
    /// Removes a league team manager by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a league team manager by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager to retrieve.</param>
    /// <returns>The matching league team manager, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeamManager?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every manager appointment belonging to the given league and team.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="teamId">The identifier of the team to filter by.</param>
    /// <returns>A list of every manager appointment belonging to the league and team.</returns>
    Task<List<LeagueTeamManager>> GetByLeagueAndTeamAsync(int leagueId, int teamId);
}
