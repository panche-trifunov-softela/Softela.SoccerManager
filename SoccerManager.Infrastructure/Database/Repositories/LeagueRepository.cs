using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="League"/> entities using Dapper.
/// </summary>
public class LeagueRepository : ILeagueRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new league and returns its generated identifier.
    /// </summary>
    /// <param name="league">The league to create.</param>
    /// <returns>The identifier assigned to the created league.</returns>
    public async Task<int> CreateAsync(League league)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", league.Name, DbType.String);
        parameters.Add("@CreatedAt", league.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", league.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", league.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", league.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeague",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing league.
    /// </summary>
    /// <param name="league">The league with updated values.</param>
    /// <returns>The identifier of the updated league.</returns>
    public async Task<int> UpdateAsync(League league)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", league.Id, DbType.Int32);
        parameters.Add("@Name", league.Name, DbType.String);
        parameters.Add("@ModifiedAt", league.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", league.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateLeague",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a league by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteLeague",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a league by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league to retrieve.</param>
    /// <returns>The matching league, or <see langword="null"/> when none exists.</returns>
    public async Task<League?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<League>(
            "dbo.GetLeagueById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all leagues.
    /// </summary>
    /// <returns>A list of every league.</returns>
    public async Task<List<League>> GetAllAsync()
    {
        var leagues = await _dapperDataContext.Connection.QueryAsync<League>(
            "dbo.GetLeagues",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return leagues.ToList();
    }
}
