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
    /// Retrieves every player registration belonging to a club's roster entry in a league.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team to filter by.</param>
    /// <returns>A list of every player registration belonging to the league team.</returns>
    Task<List<LeagueTeamPlayer>> GetByLeagueTeamIdAsync(int leagueTeamId);

    /// <summary>
    /// Retrieves a player's registration within a league.
    /// No constraint guarantees at most one registration per player per league any more; when a race left
    /// duplicates, the oldest one is returned.
    /// </summary>
    /// <param name="leagueId">The identifier of the league.</param>
    /// <param name="playerId">The identifier of the player.</param>
    /// <returns>The matching registration, or <see langword="null"/> when the player is not registered in the league.</returns>
    Task<LeagueTeamPlayer?> GetByLeagueAndPlayerAsync(int leagueId, int playerId);
}
