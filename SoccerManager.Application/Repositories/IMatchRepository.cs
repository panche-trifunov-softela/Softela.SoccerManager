using SoccerManager.Application.Models;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Match"/> entities.
/// </summary>
public interface IMatchRepository
{
    /// <summary>
    /// Persists a new match and returns its generated identifier.
    /// </summary>
    /// <param name="match">The match to create.</param>
    /// <returns>The identifier assigned to the created match.</returns>
    Task<int> CreateAsync(Match match);

    /// <summary>
    /// Persists changes to an existing match.
    /// </summary>
    /// <param name="match">The match with updated values.</param>
    /// <returns>The identifier of the updated match.</returns>
    Task<int> UpdateAsync(Match match);

    /// <summary>
    /// Removes a match by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a match by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match to retrieve.</param>
    /// <returns>The matching match, or <see langword="null"/> when none exists.</returns>
    Task<Match?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all matches.
    /// </summary>
    /// <returns>A list of every match.</returns>
    Task<List<Match>> GetAllAsync();

    /// <summary>
    /// Retrieves the earliest match of the given club that has not started, with whether the club plays at home and who the
    /// opponent is.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose next match to retrieve.</param>
    /// <returns>The club's next match, or <see langword="null"/> when it has none to come.</returns>
    Task<GetNextMatchByLeagueTeamResult?> GetNextByLeagueTeamAsync(int leagueTeamId);
}
