using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Division"/> entities using Dapper.
/// </summary>
public class DivisionRepository : IDivisionRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="DivisionRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public DivisionRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new division and returns its generated identifier.
    /// </summary>
    /// <param name="division">The division to create.</param>
    /// <returns>The identifier assigned to the created division.</returns>
    public async Task<int> CreateAsync(Division division)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", division.LeagueId, DbType.Int32);
        parameters.Add("@Name", division.Name, DbType.String);
        parameters.Add("@Order", division.Order, DbType.Int32);
        parameters.Add("@TeamsPromoted", division.TeamsPromoted, DbType.Int32);
        parameters.Add("@TeamsRelegated", division.TeamsRelegated, DbType.Int32);
        parameters.Add("@TeamsInPlayoffs", division.TeamsInPlayoffs, DbType.Int32);
        parameters.Add("@CreatedAt", division.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", division.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", division.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", division.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertDivision",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing division.
    /// </summary>
    /// <param name="division">The division with updated values.</param>
    /// <returns>The identifier of the updated division.</returns>
    public async Task<int> UpdateAsync(Division division)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", division.Id, DbType.Int32);
        parameters.Add("@Name", division.Name, DbType.String);
        parameters.Add("@Order", division.Order, DbType.Int32);
        parameters.Add("@TeamsPromoted", division.TeamsPromoted, DbType.Int32);
        parameters.Add("@TeamsRelegated", division.TeamsRelegated, DbType.Int32);
        parameters.Add("@TeamsInPlayoffs", division.TeamsInPlayoffs, DbType.Int32);
        parameters.Add("@ModifiedAt", division.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", division.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateDivision",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a division by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the division to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteDivision",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a division by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the division to retrieve.</param>
    /// <returns>The matching division, or <see langword="null"/> when none exists.</returns>
    public async Task<Division?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Division>(
            "dbo.GetDivisionById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every division belonging to the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose divisions to retrieve.</param>
    /// <returns>A list of the league's divisions.</returns>
    public async Task<List<Division>> GetByLeagueIdAsync(int leagueId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);

        var divisions = await _dapperDataContext.Connection.QueryAsync<Division>(
            "dbo.GetDivisions",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return divisions.ToList();
    }
}
