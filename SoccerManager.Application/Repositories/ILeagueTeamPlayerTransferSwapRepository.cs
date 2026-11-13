using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeamPlayerTransferSwap"/> entities.
/// </summary>
public interface ILeagueTeamPlayerTransferSwapRepository
{
    /// <summary>
    /// Persists a new swap and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferSwap">The swap to create.</param>
    /// <returns>The identifier assigned to the created swap.</returns>
    Task<int> CreateAsync(LeagueTeamPlayerTransferSwap leagueTeamPlayerTransferSwap);

    /// <summary>
    /// Persists changes to an existing swap.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferSwap">The swap with updated values.</param>
    /// <returns>The identifier of the updated swap.</returns>
    Task<int> UpdateAsync(LeagueTeamPlayerTransferSwap leagueTeamPlayerTransferSwap);

    /// <summary>
    /// Removes a swap by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the swap to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a swap by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the swap to retrieve.</param>
    /// <returns>The matching swap, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeamPlayerTransferSwap?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every swapped player of a transfer.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferId">The identifier of the transfer whose swaps to retrieve.</param>
    /// <returns>A list of every swap of the transfer.</returns>
    Task<List<LeagueTeamPlayerTransferSwap>> GetByLeagueTeamPlayerTransferIdAsync(int leagueTeamPlayerTransferId);
}
