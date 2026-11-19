using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;
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
    /// Answers a pending transfer bid on a player of the given club, by accepting or rejecting it. Accepting also rejects
    /// every other pending bid that involves a player of the deal, but moves nobody. It locks the league team rows of both
    /// clubs of the deal, the seller's and the bidder's, in Id order inside the caller's transaction, so two answers whose
    /// deals share a club cannot both pass or wait on each other.
    /// </summary>
    /// <param name="id">The identifier of the transfer bid to answer.</param>
    /// <param name="leagueTeamId">The identifier of the league team that holds the player and sells him.</param>
    /// <param name="playerId">The identifier of the player the bid is for.</param>
    /// <param name="status">The answer: <see cref="TransferStatus.Accepted"/> or <see cref="TransferStatus.Rejected"/>.</param>
    /// <param name="modifiedAt">The UTC date and time to stamp on the answered and rejected bids.</param>
    /// <param name="modifiedBy">The identifier of the user answering, to stamp on the answered and rejected bids.</param>
    /// <returns><see langword="true"/> when the answer was recorded, <see langword="false"/> when the bid does not exist, is no longer pending, is not a bid on this player at this club, the club is not its seller, or an accept is refused because a player of the deal already has an agreed deal.</returns>
    public async Task<bool> AnswerBidAsync(int id, int leagueTeamId, int playerId, TransferStatus status, DateTime modifiedAt, Guid modifiedBy)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);
        parameters.Add("@Status", (byte)status, DbType.Byte);
        parameters.Add("@ModifiedAt", modifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", modifiedBy, DbType.Guid);

        // The procedure returns the bid's identifier when it recorded the answer and 0 when it answered nothing.
        var answeredId = await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.AnswerLeagueTeamPlayerTransferBid",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return answeredId != 0;
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
