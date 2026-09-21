using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="FormationPosition"/> entities using Dapper.
/// </summary>
public class FormationPositionRepository : IFormationPositionRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormationPositionRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public FormationPositionRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new formation position and returns its generated identifier.
    /// </summary>
    /// <param name="formationPosition">The formation position to create.</param>
    /// <returns>The identifier assigned to the created formation position.</returns>
    public async Task<int> CreateAsync(FormationPosition formationPosition)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@FormationId", formationPosition.FormationId, DbType.Int32);
        parameters.Add("@PositionId", formationPosition.PositionId, DbType.Int32);
        parameters.Add("@SlotNumber", formationPosition.SlotNumber, DbType.Int32);
        parameters.Add("@CreatedAt", formationPosition.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", formationPosition.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", formationPosition.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", formationPosition.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertFormationPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing formation position.
    /// </summary>
    /// <param name="formationPosition">The formation position with updated values.</param>
    /// <returns>The identifier of the updated formation position.</returns>
    public async Task<int> UpdateAsync(FormationPosition formationPosition)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", formationPosition.Id, DbType.Int32);
        parameters.Add("@FormationId", formationPosition.FormationId, DbType.Int32);
        parameters.Add("@PositionId", formationPosition.PositionId, DbType.Int32);
        parameters.Add("@SlotNumber", formationPosition.SlotNumber, DbType.Int32);
        parameters.Add("@ModifiedAt", formationPosition.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", formationPosition.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateFormationPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a formation position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation position to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteFormationPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a formation position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation position to retrieve.</param>
    /// <returns>The matching formation position, or <see langword="null"/> when none exists.</returns>
    public async Task<FormationPosition?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<FormationPosition>(
            "dbo.GetFormationPositionById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every position slot belonging to the given formation.
    /// </summary>
    /// <param name="formationId">The identifier of the formation whose position slots to retrieve.</param>
    /// <returns>A list of every position slot belonging to the formation.</returns>
    public async Task<List<FormationPosition>> GetByFormationIdAsync(int formationId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@FormationId", formationId, DbType.Int32);

        var formationPositions = await _dapperDataContext.Connection.QueryAsync<FormationPosition>(
            "dbo.GetFormationPositions",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return formationPositions.ToList();
    }
}
