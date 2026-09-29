using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Referee"/> entities using Dapper.
/// </summary>
public class RefereeRepository : IRefereeRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefereeRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public RefereeRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new referee and returns its generated identifier.
    /// </summary>
    /// <param name="referee">The referee to create.</param>
    /// <returns>The identifier assigned to the created referee.</returns>
    public async Task<int> CreateAsync(Referee referee)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", referee.Name, DbType.String);
        parameters.Add("@ImageUrl", referee.ImageUrl, DbType.String);
        parameters.Add("@Tolerance", (byte)referee.Tolerance, DbType.Byte);
        parameters.Add("@CreatedAt", referee.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", referee.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", referee.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", referee.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertReferee",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing referee.
    /// </summary>
    /// <param name="referee">The referee with updated values.</param>
    /// <returns>The identifier of the updated referee.</returns>
    public async Task<int> UpdateAsync(Referee referee)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", referee.Id, DbType.Int32);
        parameters.Add("@Name", referee.Name, DbType.String);
        parameters.Add("@ImageUrl", referee.ImageUrl, DbType.String);
        parameters.Add("@Tolerance", (byte)referee.Tolerance, DbType.Byte);
        parameters.Add("@ModifiedAt", referee.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", referee.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateReferee",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a referee by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the referee to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteReferee",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a referee by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the referee to retrieve.</param>
    /// <returns>The matching referee, or <see langword="null"/> when none exists.</returns>
    public async Task<Referee?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Referee>(
            "dbo.GetRefereeById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all referees.
    /// </summary>
    /// <returns>A list of every referee.</returns>
    public async Task<List<Referee>> GetAllAsync()
    {
        var referees = await _dapperDataContext.Connection.QueryAsync<Referee>(
            "dbo.GetReferees",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return referees.ToList();
    }
}
