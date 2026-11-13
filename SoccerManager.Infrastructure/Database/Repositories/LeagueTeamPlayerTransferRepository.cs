using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="LeagueTeamPlayerTransfer"/> entities using Dapper.
/// </summary>
public class LeagueTeamPlayerTransferRepository : ILeagueTeamPlayerTransferRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerTransferRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueTeamPlayerTransferRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new transfer and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerTransfer">The transfer to create.</param>
    /// <returns>The identifier assigned to the created transfer.</returns>
    public async Task<int> CreateAsync(LeagueTeamPlayerTransfer leagueTeamPlayerTransfer)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerTransfer.LeagueTeamPlayerId, DbType.Int32);
        parameters.Add("@FromLeagueTeamId", leagueTeamPlayerTransfer.FromLeagueTeamId, DbType.Int32);
        parameters.Add("@ToLeagueTeamId", leagueTeamPlayerTransfer.ToLeagueTeamId, DbType.Int32);
        parameters.Add("@SeasonId", leagueTeamPlayerTransfer.SeasonId, DbType.Int32);
        parameters.Add("@TransferFee", leagueTeamPlayerTransfer.TransferFee, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@BuybackValue", leagueTeamPlayerTransfer.BuybackValue, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@BuybackValidSeasons", leagueTeamPlayerTransfer.BuybackValidSeasons, DbType.Int32);
        parameters.Add("@Status", (byte)leagueTeamPlayerTransfer.Status, DbType.Byte);
        parameters.Add("@CreatedAt", leagueTeamPlayerTransfer.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", leagueTeamPlayerTransfer.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", leagueTeamPlayerTransfer.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", leagueTeamPlayerTransfer.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeagueTeamPlayerTransfer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing transfer.
    /// </summary>
    /// <param name="leagueTeamPlayerTransfer">The transfer with updated values.</param>
    /// <returns>The identifier of the updated transfer.</returns>
    public async Task<int> UpdateAsync(LeagueTeamPlayerTransfer leagueTeamPlayerTransfer)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", leagueTeamPlayerTransfer.Id, DbType.Int32);
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerTransfer.LeagueTeamPlayerId, DbType.Int32);
        parameters.Add("@FromLeagueTeamId", leagueTeamPlayerTransfer.FromLeagueTeamId, DbType.Int32);
        parameters.Add("@ToLeagueTeamId", leagueTeamPlayerTransfer.ToLeagueTeamId, DbType.Int32);
        parameters.Add("@SeasonId", leagueTeamPlayerTransfer.SeasonId, DbType.Int32);
        parameters.Add("@TransferFee", leagueTeamPlayerTransfer.TransferFee, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@BuybackValue", leagueTeamPlayerTransfer.BuybackValue, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@BuybackValidSeasons", leagueTeamPlayerTransfer.BuybackValidSeasons, DbType.Int32);
        parameters.Add("@Status", (byte)leagueTeamPlayerTransfer.Status, DbType.Byte);
        parameters.Add("@ModifiedAt", leagueTeamPlayerTransfer.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", leagueTeamPlayerTransfer.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateLeagueTeamPlayerTransfer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a transfer by its identifier; the swap rows of the transfer go with it.
    /// </summary>
    /// <param name="id">The identifier of the transfer to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteLeagueTeamPlayerTransfer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a transfer by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the transfer to retrieve.</param>
    /// <returns>The matching transfer, or <see langword="null"/> when none exists.</returns>
    public async Task<LeagueTeamPlayerTransfer?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeamPlayerTransfer>(
            "dbo.GetLeagueTeamPlayerTransferById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every transfer of the given squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row whose transfers to retrieve.</param>
    /// <returns>A list of the squad row's transfers.</returns>
    public async Task<List<LeagueTeamPlayerTransfer>> GetByLeagueTeamPlayerIdAsync(int leagueTeamPlayerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerId, DbType.Int32);

        var leagueTeamPlayerTransfers = await _dapperDataContext.Connection.QueryAsync<LeagueTeamPlayerTransfer>(
            "dbo.GetLeagueTeamPlayerTransfers",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return leagueTeamPlayerTransfers.ToList();
    }
}
