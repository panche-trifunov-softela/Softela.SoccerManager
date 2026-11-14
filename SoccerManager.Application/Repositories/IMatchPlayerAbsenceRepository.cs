using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="MatchPlayerAbsence"/> entities.
/// </summary>
public interface IMatchPlayerAbsenceRepository
{
    /// <summary>
    /// Persists a new match player absence and returns its generated identifier.
    /// </summary>
    /// <param name="matchPlayerAbsence">The match player absence to create.</param>
    /// <returns>The identifier assigned to the created match player absence.</returns>
    Task<int> CreateAsync(MatchPlayerAbsence matchPlayerAbsence);

    /// <summary>
    /// Persists changes to an existing match player absence.
    /// </summary>
    /// <param name="matchPlayerAbsence">The match player absence with updated values.</param>
    /// <returns>The identifier of the updated match player absence.</returns>
    Task<int> UpdateAsync(MatchPlayerAbsence matchPlayerAbsence);

    /// <summary>
    /// Removes a match player absence by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player absence to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a match player absence by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player absence to retrieve.</param>
    /// <returns>The matching match player absence, or <see langword="null"/> when none exists.</returns>
    Task<MatchPlayerAbsence?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every player absence recorded for a match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose player absences to retrieve.</param>
    /// <returns>A list of every player absence recorded for the match.</returns>
    Task<List<MatchPlayerAbsence>> GetByMatchIdAsync(int matchId);
}
