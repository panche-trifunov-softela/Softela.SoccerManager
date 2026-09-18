using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeamPlayer"/> entities.
/// </summary>
public interface ILeagueTeamPlayerRepository
{
    /// <summary>
    /// Persists a new league team player and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayer">The league team player to create.</param>
    /// <returns>The identifier assigned to the created league team player.</returns>
    Task<int> CreateAsync(LeagueTeamPlayer leagueTeamPlayer);

    /// <summary>
    /// Persists changes to an existing league team player.
    /// </summary>
    /// <param name="leagueTeamPlayer">The league team player with updated values.</param>
    /// <returns>The identifier of the updated league team player.</returns>
    Task<int> UpdateAsync(LeagueTeamPlayer leagueTeamPlayer);

    /// <summary>
    /// Removes a league team player by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team player to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a league team player by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team player to retrieve.</param>
    /// <returns>The matching league team player, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeamPlayer?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every player registration belonging to a team within a league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="teamId">The identifier of the team to filter by.</param>
    /// <returns>A list of every player registration belonging to the league and team.</returns>
    Task<List<LeagueTeamPlayer>> GetByLeagueAndTeamAsync(int leagueId, int teamId);
}
