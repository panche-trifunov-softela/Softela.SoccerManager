using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Stadium"/> entities using Dapper.
/// </summary>
public class StadiumRepository : IStadiumRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="StadiumRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public StadiumRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new stadium and returns its generated identifier.
    /// </summary>
    /// <param name="stadium">The stadium to create.</param>
    /// <returns>The identifier assigned to the created stadium.</returns>
    public async Task<int> CreateAsync(Stadium stadium)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", stadium.Name, DbType.String);
        parameters.Add("@ImageUrl", stadium.ImageUrl, DbType.String);
        parameters.Add("@Size", stadium.Size, DbType.Int32);
        parameters.Add("@CreatedAt", stadium.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", stadium.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", stadium.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", stadium.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertStadium",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing stadium.
    /// </summary>
    /// <param name="stadium">The stadium with updated values.</param>
    /// <returns>The identifier of the updated stadium.</returns>
    public async Task<int> UpdateAsync(Stadium stadium)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", stadium.Id, DbType.Int32);
        parameters.Add("@Name", stadium.Name, DbType.String);
        parameters.Add("@ImageUrl", stadium.ImageUrl, DbType.String);
        parameters.Add("@Size", stadium.Size, DbType.Int32);
        parameters.Add("@ModifiedAt", stadium.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", stadium.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateStadium",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a stadium by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the stadium to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteStadium",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a stadium by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the stadium to retrieve.</param>
    /// <returns>The matching stadium, or <see langword="null"/> when none exists.</returns>
    public async Task<Stadium?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Stadium>(
            "dbo.GetStadiumById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all stadiums.
    /// </summary>
    /// <returns>A list of every stadium.</returns>
    public async Task<List<Stadium>> GetAllAsync()
    {
        var stadiums = await _dapperDataContext.Connection.QueryAsync<Stadium>(
            "dbo.GetStadiums",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return stadiums.ToList();
    }
}
