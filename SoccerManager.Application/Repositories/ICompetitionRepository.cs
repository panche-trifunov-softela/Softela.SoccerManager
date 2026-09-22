using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Competition"/> entities.
/// </summary>
public interface ICompetitionRepository
{
    /// <summary>
    /// Persists a new competition and returns its generated identifier.
    /// </summary>
    /// <param name="competition">The competition to create.</param>
    /// <returns>The identifier assigned to the created competition.</returns>
    Task<int> CreateAsync(Competition competition);

    /// <summary>
    /// Persists changes to an existing competition.
    /// </summary>
    /// <param name="competition">The competition with updated values.</param>
    /// <returns>The identifier of the updated competition.</returns>
    Task<int> UpdateAsync(Competition competition);

    /// <summary>
    /// Removes a competition by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the competition to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a competition by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the competition to retrieve.</param>
    /// <returns>The matching competition, or <see langword="null"/> when none exists.</returns>
    Task<Competition?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every competition belonging to the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose competitions to retrieve.</param>
    /// <returns>A list of every competition belonging to the league.</returns>
    Task<List<Competition>> GetByLeagueIdAsync(int leagueId);
}
