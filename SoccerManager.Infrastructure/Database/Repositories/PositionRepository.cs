using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Position"/> entities using Dapper.
/// </summary>
public class PositionRepository : IPositionRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PositionRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public PositionRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new position and returns its generated identifier.
    /// </summary>
    /// <param name="position">The position to create.</param>
    /// <returns>The identifier assigned to the created position.</returns>
    public async Task<int> CreateAsync(Position position)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", position.Name, DbType.String);
        parameters.Add("@Area", (byte)position.Area, DbType.Byte);
        parameters.Add("@Side", (byte)position.Side, DbType.Byte);
        parameters.Add("@CreatedAt", position.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", position.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", position.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", position.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing position.
    /// </summary>
    /// <param name="position">The position with updated values.</param>
    /// <returns>The identifier of the updated position.</returns>
    public async Task<int> UpdateAsync(Position position)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", position.Id, DbType.Int32);
        parameters.Add("@Name", position.Name, DbType.String);
        parameters.Add("@Area", (byte)position.Area, DbType.Byte);
        parameters.Add("@Side", (byte)position.Side, DbType.Byte);
        parameters.Add("@ModifiedAt", position.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", position.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdatePosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the position to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeletePosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the position to retrieve.</param>
    /// <returns>The matching position, or <see langword="null"/> when none exists.</returns>
    public async Task<Position?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Position>(
            "dbo.GetPositionById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all positions.
    /// </summary>
    /// <returns>A list of every position.</returns>
    public async Task<List<Position>> GetAllAsync()
    {
        var positions = await _dapperDataContext.Connection.QueryAsync<Position>(
            "dbo.GetPositions",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return positions.ToList();
    }
}
