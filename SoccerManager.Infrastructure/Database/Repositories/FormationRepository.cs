using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Formation"/> entities using Dapper.
/// </summary>
public class FormationRepository : IFormationRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormationRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public FormationRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new formation and returns its generated identifier.
    /// </summary>
    /// <param name="formation">The formation to create.</param>
    /// <returns>The identifier assigned to the created formation.</returns>
    public async Task<int> CreateAsync(Formation formation)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", formation.Name, DbType.String);
        parameters.Add("@CreatedAt", formation.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", formation.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", formation.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", formation.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertFormation",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing formation.
    /// </summary>
    /// <param name="formation">The formation with updated values.</param>
    /// <returns>The identifier of the updated formation.</returns>
    public async Task<int> UpdateAsync(Formation formation)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", formation.Id, DbType.Int32);
        parameters.Add("@Name", formation.Name, DbType.String);
        parameters.Add("@ModifiedAt", formation.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", formation.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateFormation",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a formation by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteFormation",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a formation by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation to retrieve.</param>
    /// <returns>The matching formation, or <see langword="null"/> when none exists.</returns>
    public async Task<Formation?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Formation>(
            "dbo.GetFormationById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all formations.
    /// </summary>
    /// <returns>A list of every formation.</returns>
    public async Task<List<Formation>> GetAllAsync()
    {
        var formations = await _dapperDataContext.Connection.QueryAsync<Formation>(
            "dbo.GetFormations",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return formations.ToList();
    }

    /// <summary>
    /// Retrieves a formation by its name.
    /// </summary>
    /// <param name="name">The name to match, compared case-insensitively by the database collation.</param>
    /// <returns>The matching formation, or <see langword="null"/> when none exists.</returns>
    public async Task<Formation?> GetByNameAsync(string name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", name, DbType.String);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Formation>(
            "dbo.GetFormationByName",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
