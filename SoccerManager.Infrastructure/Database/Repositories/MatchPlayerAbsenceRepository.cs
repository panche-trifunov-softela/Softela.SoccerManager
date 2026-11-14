using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="MatchPlayerAbsence"/> entities using Dapper.
/// </summary>
public class MatchPlayerAbsenceRepository : IMatchPlayerAbsenceRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchPlayerAbsenceRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public MatchPlayerAbsenceRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new match player absence and returns its generated identifier.
    /// </summary>
    /// <param name="matchPlayerAbsence">The match player absence to create.</param>
    /// <returns>The identifier assigned to the created match player absence.</returns>
    public async Task<int> CreateAsync(MatchPlayerAbsence matchPlayerAbsence)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchPlayerAbsence.MatchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", matchPlayerAbsence.LeagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", matchPlayerAbsence.PlayerId, DbType.Int32);
        parameters.Add("@Reason", (byte)matchPlayerAbsence.Reason, DbType.Byte);
        parameters.Add("@CreatedAt", matchPlayerAbsence.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", matchPlayerAbsence.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", matchPlayerAbsence.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", matchPlayerAbsence.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertMatchPlayerAbsence",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing match player absence.
    /// </summary>
    /// <param name="matchPlayerAbsence">The match player absence with updated values.</param>
    /// <returns>The identifier of the updated match player absence.</returns>
    public async Task<int> UpdateAsync(MatchPlayerAbsence matchPlayerAbsence)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", matchPlayerAbsence.Id, DbType.Int32);
        parameters.Add("@MatchId", matchPlayerAbsence.MatchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", matchPlayerAbsence.LeagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", matchPlayerAbsence.PlayerId, DbType.Int32);
        parameters.Add("@Reason", (byte)matchPlayerAbsence.Reason, DbType.Byte);
        parameters.Add("@ModifiedAt", matchPlayerAbsence.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", matchPlayerAbsence.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateMatchPlayerAbsence",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a match player absence by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player absence to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatchPlayerAbsence",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a match player absence by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player absence to retrieve.</param>
    /// <returns>The matching match player absence, or <see langword="null"/> when none exists.</returns>
    public async Task<MatchPlayerAbsence?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<MatchPlayerAbsence>(
            "dbo.GetMatchPlayerAbsenceById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every player absence recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose player absences to retrieve.</param>
    /// <returns>A list of the match's player absences.</returns>
    public async Task<List<MatchPlayerAbsence>> GetByMatchIdAsync(int matchId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);

        var matchPlayerAbsences = await _dapperDataContext.Connection.QueryAsync<MatchPlayerAbsence>(
            "dbo.GetMatchPlayerAbsences",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return matchPlayerAbsences.ToList();
    }
}
