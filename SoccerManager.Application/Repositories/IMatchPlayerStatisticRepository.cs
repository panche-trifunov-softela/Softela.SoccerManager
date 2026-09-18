using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="MatchPlayerStatistic"/> entities.
/// </summary>
public interface IMatchPlayerStatisticRepository
{
    /// <summary>
    /// Persists a new match player statistic and returns its generated identifier.
    /// </summary>
    /// <param name="matchPlayerStatistic">The match player statistic to create.</param>
    /// <returns>The identifier assigned to the created match player statistic.</returns>
    Task<int> CreateAsync(MatchPlayerStatistic matchPlayerStatistic);

    /// <summary>
    /// Persists changes to an existing match player statistic.
    /// </summary>
    /// <param name="matchPlayerStatistic">The match player statistic with updated values.</param>
    /// <returns>The identifier of the updated match player statistic.</returns>
    Task<int> UpdateAsync(MatchPlayerStatistic matchPlayerStatistic);

    /// <summary>
    /// Removes a match player statistic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player statistic to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a match player statistic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player statistic to retrieve.</param>
    /// <returns>The matching match player statistic, or <see langword="null"/> when none exists.</returns>
    Task<MatchPlayerStatistic?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every player statistic recorded for a match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose player statistics to retrieve.</param>
    /// <returns>A list of every player statistic recorded for the match.</returns>
    Task<List<MatchPlayerStatistic>> GetByMatchIdAsync(int matchId);
}
