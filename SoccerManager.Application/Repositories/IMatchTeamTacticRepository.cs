using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="MatchTeamTactic"/> entities.
/// </summary>
public interface IMatchTeamTacticRepository
{
    /// <summary>
    /// Persists a new match team tactic and returns its generated identifier.
    /// </summary>
    /// <param name="matchTeamTactic">The match team tactic to create.</param>
    /// <returns>The identifier assigned to the created match team tactic.</returns>
    Task<int> CreateAsync(MatchTeamTactic matchTeamTactic);

    /// <summary>
    /// Persists changes to an existing match team tactic.
    /// </summary>
    /// <param name="matchTeamTactic">The match team tactic with updated values.</param>
    /// <returns>The identifier of the updated match team tactic.</returns>
    Task<int> UpdateAsync(MatchTeamTactic matchTeamTactic);

    /// <summary>
    /// Removes a match team tactic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team tactic to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a match team tactic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team tactic to retrieve.</param>
    /// <returns>The matching match team tactic, or <see langword="null"/> when none exists.</returns>
    Task<MatchTeamTactic?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every team tactic recorded for a match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose team tactics to retrieve.</param>
    /// <returns>A list of every team tactic recorded for the match.</returns>
    Task<List<MatchTeamTactic>> GetByMatchIdAsync(int matchId);
}
