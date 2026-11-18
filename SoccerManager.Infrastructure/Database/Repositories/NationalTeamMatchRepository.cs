using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="NationalTeamMatch"/> entities using Dapper.
/// </summary>
public class NationalTeamMatchRepository : INationalTeamMatchRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="NationalTeamMatchRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public NationalTeamMatchRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new national team match and returns its generated identifier.
    /// </summary>
    /// <param name="nationalTeamMatch">The national team match to create.</param>
    /// <returns>The identifier assigned to the created national team match.</returns>
    public async Task<int> CreateAsync(NationalTeamMatch nationalTeamMatch)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SeasonId", nationalTeamMatch.SeasonId, DbType.Int32);
        parameters.Add("@HomeNationalTeamId", nationalTeamMatch.HomeNationalTeamId, DbType.Int32);
        parameters.Add("@AwayNationalTeamId", nationalTeamMatch.AwayNationalTeamId, DbType.Int32);
        parameters.Add("@StartDateTime", nationalTeamMatch.StartDateTime, DbType.DateTime2);
        parameters.Add("@Stage", (byte)nationalTeamMatch.Stage, DbType.Byte);
        parameters.Add("@GroupNumber", nationalTeamMatch.GroupNumber, DbType.Byte);
        parameters.Add("@RoundNumber", nationalTeamMatch.RoundNumber, DbType.Byte);
        parameters.Add("@Leg", nationalTeamMatch.Leg, DbType.Byte);
        parameters.Add("@IsStarted", nationalTeamMatch.IsStarted, DbType.Boolean);
        parameters.Add("@CreatedAt", nationalTeamMatch.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", nationalTeamMatch.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", nationalTeamMatch.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", nationalTeamMatch.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertNationalTeamMatch",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing national team match.
    /// </summary>
    /// <param name="nationalTeamMatch">The national team match with updated values.</param>
    /// <returns>The identifier of the updated national team match.</returns>
    public async Task<int> UpdateAsync(NationalTeamMatch nationalTeamMatch)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", nationalTeamMatch.Id, DbType.Int32);
        parameters.Add("@SeasonId", nationalTeamMatch.SeasonId, DbType.Int32);
        parameters.Add("@HomeNationalTeamId", nationalTeamMatch.HomeNationalTeamId, DbType.Int32);
        parameters.Add("@AwayNationalTeamId", nationalTeamMatch.AwayNationalTeamId, DbType.Int32);
        parameters.Add("@StartDateTime", nationalTeamMatch.StartDateTime, DbType.DateTime2);
        parameters.Add("@Stage", (byte)nationalTeamMatch.Stage, DbType.Byte);
        parameters.Add("@GroupNumber", nationalTeamMatch.GroupNumber, DbType.Byte);
        parameters.Add("@RoundNumber", nationalTeamMatch.RoundNumber, DbType.Byte);
        parameters.Add("@Leg", nationalTeamMatch.Leg, DbType.Byte);
        parameters.Add("@IsStarted", nationalTeamMatch.IsStarted, DbType.Boolean);
        parameters.Add("@IsFinished", nationalTeamMatch.IsFinished, DbType.Boolean);
        parameters.Add("@ModifiedAt", nationalTeamMatch.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", nationalTeamMatch.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateNationalTeamMatch",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a national team match by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team match to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteNationalTeamMatch",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a national team match by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team match to retrieve.</param>
    /// <returns>The matching national team match, or <see langword="null"/> when none exists.</returns>
    public async Task<NationalTeamMatch?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<NationalTeamMatch>(
            "dbo.GetNationalTeamMatchById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every national team match of the given season, in kickoff order.
    /// </summary>
    /// <param name="seasonId">The identifier of the season whose national team matches to retrieve.</param>
    /// <returns>A list of the season's national team matches, in kickoff order.</returns>
    public async Task<List<NationalTeamMatch>> GetBySeasonIdAsync(int seasonId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SeasonId", seasonId, DbType.Int32);

        var nationalTeamMatches = await _dapperDataContext.Connection.QueryAsync<NationalTeamMatch>(
            "dbo.GetNationalTeamMatches",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return nationalTeamMatches.ToList();
    }
}
