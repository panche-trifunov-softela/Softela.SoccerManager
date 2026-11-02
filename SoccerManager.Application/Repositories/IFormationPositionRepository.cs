using SoccerManager.Application.Models;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="FormationPosition"/> entities.
/// </summary>
public interface IFormationPositionRepository
{
    /// <summary>
    /// Persists a new formation position and returns its generated identifier.
    /// </summary>
    /// <param name="formationPosition">The formation position to create.</param>
    /// <returns>The identifier assigned to the created formation position.</returns>
    Task<int> CreateAsync(FormationPosition formationPosition);

    /// <summary>
    /// Persists changes to an existing formation position.
    /// </summary>
    /// <param name="formationPosition">The formation position with updated values.</param>
    /// <returns>The identifier of the updated formation position.</returns>
    Task<int> UpdateAsync(FormationPosition formationPosition);

    /// <summary>
    /// Removes a formation position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation position to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a formation position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation position to retrieve.</param>
    /// <returns>The matching formation position, or <see langword="null"/> when none exists.</returns>
    Task<FormationPosition?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every position slot belonging to the given formation.
    /// </summary>
    /// <param name="formationId">The identifier of the formation whose position slots to retrieve.</param>
    /// <returns>A list of every position slot belonging to the formation.</returns>
    Task<List<FormationPosition>> GetByFormationIdAsync(int formationId);

    /// <summary>
    /// Writes the pitch position of a formation position onto its row; two <see langword="null"/> coordinates clear it. Only the
    /// two pitch columns change.
    /// </summary>
    /// <param name="formationPosition">The formation position whose identifier and <see cref="FormationPosition.PitchX"/> and <see cref="FormationPosition.PitchY"/> to write.</param>
    Task SetPitchAsync(FormationPosition formationPosition);

    /// <summary>
    /// Retrieves every slot of the given formation with its position and its place on the pitch, in slot order.
    /// </summary>
    /// <param name="formationId">The identifier of the formation whose slots to retrieve.</param>
    /// <returns>A list of the formation's slots, empty when the formation is unknown or has no slots.</returns>
    Task<List<GetFormationSlotsResult>> GetSlotsAsync(int formationId);
}
