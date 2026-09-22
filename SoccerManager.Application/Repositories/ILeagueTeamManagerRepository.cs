using SoccerManager.Application.Models;
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

    /// <summary>
    /// Retrieves every manager appointment belonging to the manager profile linked to the given Keycloak user, enriched with league and team names.
    /// </summary>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose appointments are retrieved.</param>
    /// <param name="currentOnly">When <see langword="true"/>, narrows the results to appointments where <c>IsCurrent</c> is <see langword="true"/>.</param>
    /// <returns>A list of the user's manager appointments, or an empty list when the user has no manager profile.</returns>
    Task<List<GetLeagueTeamManagersByUserIdResult>> GetByUserIdAsync(Guid userId, bool currentOnly);
}
