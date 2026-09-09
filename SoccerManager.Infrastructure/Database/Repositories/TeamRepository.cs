using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Team"/> entities using Dapper.
/// </summary>
public class TeamRepository : ITeamRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public TeamRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new team and returns its generated identifier.
    /// </summary>
    /// <param name="team">The team to create.</param>
    /// <returns>The identifier assigned to the created team.</returns>
    public async Task<int> CreateAsync(Team team)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", team.Name, DbType.String);
        parameters.Add("@StadiumId", team.StadiumId, DbType.Int32);
        parameters.Add("@FinancialState", (byte)team.FinancialState, DbType.Byte);
        parameters.Add("@JerseyUrl", team.JerseyUrl, DbType.String);
        parameters.Add("@CreatedAt", team.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", team.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", team.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", team.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing team.
    /// </summary>
    /// <param name="team">The team with updated values.</param>
    /// <returns>The identifier of the updated team.</returns>
    public async Task<int> UpdateAsync(Team team)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", team.Id, DbType.Int32);
        parameters.Add("@Name", team.Name, DbType.String);
        parameters.Add("@StadiumId", team.StadiumId, DbType.Int32);
        parameters.Add("@FinancialState", (byte)team.FinancialState, DbType.Byte);
        parameters.Add("@JerseyUrl", team.JerseyUrl, DbType.String);
        parameters.Add("@ModifiedAt", team.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", team.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the team to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the team to retrieve.</param>
    /// <returns>The matching team, or <see langword="null"/> when none exists.</returns>
    public async Task<Team?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Team>(
            "dbo.GetTeamById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all teams.
    /// </summary>
    /// <returns>A list of every team.</returns>
    public async Task<List<Team>> GetAllAsync()
    {
        var teams = await _dapperDataContext.Connection.QueryAsync<Team>(
            "dbo.GetTeams",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return teams.ToList();
    }
}
