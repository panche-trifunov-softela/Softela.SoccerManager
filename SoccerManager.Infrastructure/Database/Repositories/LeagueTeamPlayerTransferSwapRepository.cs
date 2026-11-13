using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="LeagueTeamPlayerTransferSwap"/> entities using Dapper.
/// </summary>
public class LeagueTeamPlayerTransferSwapRepository : ILeagueTeamPlayerTransferSwapRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerTransferSwapRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueTeamPlayerTransferSwapRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new swap and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferSwap">The swap to create.</param>
    /// <returns>The identifier assigned to the created swap.</returns>
    public async Task<int> CreateAsync(LeagueTeamPlayerTransferSwap leagueTeamPlayerTransferSwap)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamPlayerTransferId", leagueTeamPlayerTransferSwap.LeagueTeamPlayerTransferId, DbType.Int32);
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerTransferSwap.LeagueTeamPlayerId, DbType.Int32);
        parameters.Add("@CreatedAt", leagueTeamPlayerTransferSwap.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", leagueTeamPlayerTransferSwap.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", leagueTeamPlayerTransferSwap.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", leagueTeamPlayerTransferSwap.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeagueTeamPlayerTransferSwap",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing swap.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferSwap">The swap with updated values.</param>
    /// <returns>The identifier of the updated swap.</returns>
    public async Task<int> UpdateAsync(LeagueTeamPlayerTransferSwap leagueTeamPlayerTransferSwap)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", leagueTeamPlayerTransferSwap.Id, DbType.Int32);
        parameters.Add("@LeagueTeamPlayerTransferId", leagueTeamPlayerTransferSwap.LeagueTeamPlayerTransferId, DbType.Int32);
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerTransferSwap.LeagueTeamPlayerId, DbType.Int32);
        parameters.Add("@ModifiedAt", leagueTeamPlayerTransferSwap.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", leagueTeamPlayerTransferSwap.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateLeagueTeamPlayerTransferSwap",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a swap by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the swap to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteLeagueTeamPlayerTransferSwap",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a swap by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the swap to retrieve.</param>
    /// <returns>The matching swap, or <see langword="null"/> when none exists.</returns>
    public async Task<LeagueTeamPlayerTransferSwap?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeamPlayerTransferSwap>(
            "dbo.GetLeagueTeamPlayerTransferSwapById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every swapped player of the given transfer.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferId">The identifier of the transfer whose swaps to retrieve.</param>
    /// <returns>A list of the transfer's swaps.</returns>
    public async Task<List<LeagueTeamPlayerTransferSwap>> GetByLeagueTeamPlayerTransferIdAsync(int leagueTeamPlayerTransferId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamPlayerTransferId", leagueTeamPlayerTransferId, DbType.Int32);

        var leagueTeamPlayerTransferSwaps = await _dapperDataContext.Connection.QueryAsync<LeagueTeamPlayerTransferSwap>(
            "dbo.GetLeagueTeamPlayerTransferSwaps",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return leagueTeamPlayerTransferSwaps.ToList();
    }
}
