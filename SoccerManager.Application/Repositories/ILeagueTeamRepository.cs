using SoccerManager.Application.Models;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeam"/> entities.
/// </summary>
public interface ILeagueTeamRepository
{
    /// <summary>
    /// Persists a new league team and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeam">The league team to create.</param>
    /// <returns>The identifier assigned to the created league team.</returns>
    Task<int> CreateAsync(LeagueTeam leagueTeam);

    /// <summary>
    /// Removes a league team by its identifier, but only when nothing is still attached to the club in
    /// that league. The check and the delete are atomic inside the stored procedure.
    /// </summary>
    /// <param name="id">The identifier of the league team to delete.</param>
    /// <returns>
    /// Whether the league team was removed and, when it was not, which kinds of record still block it.
    /// When nothing is flagged as blocking, the league team did not exist.
    /// </returns>
    Task<DeleteLeagueTeamResult> DeleteAsync(int id);

    /// <summary>
    /// Retrieves a league team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team to retrieve.</param>
    /// <returns>The matching league team, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeam?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every team on the roster of the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to retrieve.</param>
    /// <returns>A list of every league team belonging to the league.</returns>
    Task<List<LeagueTeam>> GetByLeagueIdAsync(int leagueId);

    /// <summary>
    /// Retrieves the roster entry of the given team in the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to look in.</param>
    /// <param name="teamId">The identifier of the team to look for.</param>
    /// <returns>The matching league team, or <see langword="null"/> when the team is not on the league's roster.</returns>
    Task<LeagueTeam?> GetByLeagueAndTeamAsync(int leagueId, int teamId);
}
