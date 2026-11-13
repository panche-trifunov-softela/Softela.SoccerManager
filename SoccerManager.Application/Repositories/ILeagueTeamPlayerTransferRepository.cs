using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeamPlayerTransfer"/> entities.
/// </summary>
public interface ILeagueTeamPlayerTransferRepository
{
    /// <summary>
    /// Persists a new transfer and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerTransfer">The transfer to create.</param>
    /// <returns>The identifier assigned to the created transfer.</returns>
    Task<int> CreateAsync(LeagueTeamPlayerTransfer leagueTeamPlayerTransfer);

    /// <summary>
    /// Persists changes to an existing transfer.
    /// </summary>
    /// <param name="leagueTeamPlayerTransfer">The transfer with updated values.</param>
    /// <returns>The identifier of the updated transfer.</returns>
    Task<int> UpdateAsync(LeagueTeamPlayerTransfer leagueTeamPlayerTransfer);

    /// <summary>
    /// Removes a transfer by its identifier; the swap rows of the transfer go with it.
    /// </summary>
    /// <param name="id">The identifier of the transfer to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a transfer by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the transfer to retrieve.</param>
    /// <returns>The matching transfer, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeamPlayerTransfer?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every transfer of a squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row whose transfers to retrieve.</param>
    /// <returns>A list of every transfer of the squad row.</returns>
    Task<List<LeagueTeamPlayerTransfer>> GetByLeagueTeamPlayerIdAsync(int leagueTeamPlayerId);
}
