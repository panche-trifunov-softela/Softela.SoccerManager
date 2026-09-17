using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Manager"/> entities using Dapper.
/// </summary>
public class ManagerRepository : IManagerRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagerRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public ManagerRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new manager and returns its generated identifier.
    /// </summary>
    /// <param name="manager">The manager to create.</param>
    /// <returns>The identifier assigned to the created manager.</returns>
    public async Task<int> CreateAsync(Manager manager)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", manager.UserId, DbType.Guid);
        parameters.Add("@ImageUrl", manager.ImageUrl, DbType.String);
        parameters.Add("@CreatedAt", manager.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", manager.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", manager.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", manager.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertManager",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing manager.
    /// </summary>
    /// <param name="manager">The manager with updated values.</param>
    /// <returns>The identifier of the updated manager.</returns>
    public async Task<int> UpdateAsync(Manager manager)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", manager.Id, DbType.Int32);
        parameters.Add("@ImageUrl", manager.ImageUrl, DbType.String);
        parameters.Add("@ModifiedAt", manager.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", manager.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateManager",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a manager by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the manager to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteManager",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a manager by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the manager to retrieve.</param>
    /// <returns>The matching manager, or <see langword="null"/> when none exists.</returns>
    public async Task<Manager?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Manager>(
            "dbo.GetManagerById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all managers.
    /// </summary>
    /// <returns>A list of every manager.</returns>
    public async Task<List<Manager>> GetAllAsync()
    {
        var managers = await _dapperDataContext.Connection.QueryAsync<Manager>(
            "dbo.GetManagers",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return managers.ToList();
    }
}
