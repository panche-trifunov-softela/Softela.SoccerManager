using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="MatchTeamStatistic"/> entities.
/// </summary>
public interface IMatchTeamStatisticRepository
{
    /// <summary>
    /// Persists a new match team statistic and returns its generated identifier.
    /// </summary>
    /// <param name="matchTeamStatistic">The match team statistic to create.</param>
    /// <returns>The identifier assigned to the created match team statistic.</returns>
    Task<int> CreateAsync(MatchTeamStatistic matchTeamStatistic);

    /// <summary>
    /// Persists changes to an existing match team statistic.
    /// </summary>
    /// <param name="matchTeamStatistic">The match team statistic with updated values.</param>
    /// <returns>The identifier of the updated match team statistic.</returns>
    Task<int> UpdateAsync(MatchTeamStatistic matchTeamStatistic);

    /// <summary>
    /// Removes a match team statistic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team statistic to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a match team statistic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team statistic to retrieve.</param>
    /// <returns>The matching match team statistic, or <see langword="null"/> when none exists.</returns>
    Task<MatchTeamStatistic?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every team statistic recorded for a match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose team statistics to retrieve.</param>
    /// <returns>A list of every team statistic recorded for the match.</returns>
    Task<List<MatchTeamStatistic>> GetByMatchIdAsync(int matchId);
}
