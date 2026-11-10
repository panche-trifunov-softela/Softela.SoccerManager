using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="PenaltyAfterExtraTime"/> entities using Dapper.
/// </summary>
public class PenaltyAfterExtraTimeRepository : IPenaltyAfterExtraTimeRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PenaltyAfterExtraTimeRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public PenaltyAfterExtraTimeRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new penalty after extra time and returns its generated identifier.
    /// </summary>
    /// <param name="penaltyAfterExtraTime">The penalty after extra time to create.</param>
    /// <returns>The identifier assigned to the created penalty after extra time.</returns>
    public async Task<int> CreateAsync(PenaltyAfterExtraTime penaltyAfterExtraTime)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", penaltyAfterExtraTime.MatchId, DbType.Int32);
        parameters.Add("@KickNumber", penaltyAfterExtraTime.KickNumber, DbType.Int32);
        parameters.Add("@LeagueTeamId", penaltyAfterExtraTime.LeagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", penaltyAfterExtraTime.PlayerId, DbType.Int32);
        parameters.Add("@IsScored", penaltyAfterExtraTime.IsScored, DbType.Boolean);
        parameters.Add("@CreatedAt", penaltyAfterExtraTime.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", penaltyAfterExtraTime.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", penaltyAfterExtraTime.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", penaltyAfterExtraTime.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertPenaltyAfterExtraTime",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing penalty after extra time.
    /// </summary>
    /// <param name="penaltyAfterExtraTime">The penalty after extra time with updated values.</param>
    /// <returns>The identifier of the updated penalty after extra time.</returns>
    public async Task<int> UpdateAsync(PenaltyAfterExtraTime penaltyAfterExtraTime)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", penaltyAfterExtraTime.Id, DbType.Int32);
        parameters.Add("@MatchId", penaltyAfterExtraTime.MatchId, DbType.Int32);
        parameters.Add("@KickNumber", penaltyAfterExtraTime.KickNumber, DbType.Int32);
        parameters.Add("@LeagueTeamId", penaltyAfterExtraTime.LeagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", penaltyAfterExtraTime.PlayerId, DbType.Int32);
        parameters.Add("@IsScored", penaltyAfterExtraTime.IsScored, DbType.Boolean);
        parameters.Add("@ModifiedAt", penaltyAfterExtraTime.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", penaltyAfterExtraTime.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdatePenaltyAfterExtraTime",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a penalty after extra time by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the penalty after extra time to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeletePenaltyAfterExtraTime",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a penalty after extra time by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the penalty after extra time to retrieve.</param>
    /// <returns>The matching penalty after extra time, or <see langword="null"/> when none exists.</returns>
    public async Task<PenaltyAfterExtraTime?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<PenaltyAfterExtraTime>(
            "dbo.GetPenaltyAfterExtraTimeById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every kick of the shootout of the given match, in kick order.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose shootout kicks to retrieve.</param>
    /// <returns>A list of the match's shootout kicks, in kick order.</returns>
    public async Task<List<PenaltyAfterExtraTime>> GetByMatchIdAsync(int matchId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);

        var penaltiesAfterExtraTime = await _dapperDataContext.Connection.QueryAsync<PenaltyAfterExtraTime>(
            "dbo.GetPenaltiesAfterExtraTime",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return penaltiesAfterExtraTime.ToList();
    }
}
