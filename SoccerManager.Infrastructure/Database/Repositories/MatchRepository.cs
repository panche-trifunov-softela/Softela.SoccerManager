using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Match"/> entities using Dapper.
/// </summary>
public class MatchRepository : IMatchRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public MatchRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new match and returns its generated identifier.
    /// </summary>
    /// <param name="match">The match to create.</param>
    /// <returns>The identifier assigned to the created match.</returns>
    public async Task<int> CreateAsync(Match match)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SeasonId", match.SeasonId, DbType.Int32);
        parameters.Add("@DivisionId", match.DivisionId, DbType.Int32);
        parameters.Add("@RefereeId", match.RefereeId, DbType.Int32);
        parameters.Add("@StartDateTime", match.StartDateTime, DbType.DateTime2);
        parameters.Add("@Commentary", match.Commentary, DbType.String);
        parameters.Add("@Attendance", match.Attendance, DbType.Int32);
        parameters.Add("@CreatedAt", match.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", match.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", match.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", match.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertMatch",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing match.
    /// </summary>
    /// <param name="match">The match with updated values.</param>
    /// <returns>The identifier of the updated match.</returns>
    public async Task<int> UpdateAsync(Match match)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", match.Id, DbType.Int32);
        parameters.Add("@SeasonId", match.SeasonId, DbType.Int32);
        parameters.Add("@DivisionId", match.DivisionId, DbType.Int32);
        parameters.Add("@RefereeId", match.RefereeId, DbType.Int32);
        parameters.Add("@StartDateTime", match.StartDateTime, DbType.DateTime2);
        parameters.Add("@Commentary", match.Commentary, DbType.String);
        parameters.Add("@Attendance", match.Attendance, DbType.Int32);
        parameters.Add("@ModifiedAt", match.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", match.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateMatch",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a match by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatch",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a match by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match to retrieve.</param>
    /// <returns>The matching match, or <see langword="null"/> when none exists.</returns>
    public async Task<Match?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Match>(
            "dbo.GetMatchById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all matches.
    /// </summary>
    /// <returns>A list of every match.</returns>
    public async Task<List<Match>> GetAllAsync()
    {
        var matches = await _dapperDataContext.Connection.QueryAsync<Match>(
            "dbo.GetMatches",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return matches.ToList();
    }
}
