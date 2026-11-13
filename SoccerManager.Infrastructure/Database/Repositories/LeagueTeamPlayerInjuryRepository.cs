using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="LeagueTeamPlayerInjury"/> entities using Dapper.
/// </summary>
public class LeagueTeamPlayerInjuryRepository : ILeagueTeamPlayerInjuryRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerInjuryRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueTeamPlayerInjuryRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new injury and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerInjury">The injury to create.</param>
    /// <returns>The identifier assigned to the created injury.</returns>
    public async Task<int> CreateAsync(LeagueTeamPlayerInjury leagueTeamPlayerInjury)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerInjury.LeagueTeamPlayerId, DbType.Int32);
        parameters.Add("@Name", leagueTeamPlayerInjury.Name, DbType.String);
        parameters.Add("@InjuryStartDate", leagueTeamPlayerInjury.InjuryStartDate, DbType.DateTime2);
        parameters.Add("@InjuryEndDate", leagueTeamPlayerInjury.InjuryEndDate, DbType.DateTime2);
        parameters.Add("@InjuryScale", (byte)leagueTeamPlayerInjury.InjuryScale, DbType.Byte);
        parameters.Add("@CreatedAt", leagueTeamPlayerInjury.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", leagueTeamPlayerInjury.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", leagueTeamPlayerInjury.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", leagueTeamPlayerInjury.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeagueTeamPlayerInjury",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing injury.
    /// </summary>
    /// <param name="leagueTeamPlayerInjury">The injury with updated values.</param>
    /// <returns>The identifier of the updated injury.</returns>
    public async Task<int> UpdateAsync(LeagueTeamPlayerInjury leagueTeamPlayerInjury)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", leagueTeamPlayerInjury.Id, DbType.Int32);
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerInjury.LeagueTeamPlayerId, DbType.Int32);
        parameters.Add("@Name", leagueTeamPlayerInjury.Name, DbType.String);
        parameters.Add("@InjuryStartDate", leagueTeamPlayerInjury.InjuryStartDate, DbType.DateTime2);
        parameters.Add("@InjuryEndDate", leagueTeamPlayerInjury.InjuryEndDate, DbType.DateTime2);
        parameters.Add("@InjuryScale", (byte)leagueTeamPlayerInjury.InjuryScale, DbType.Byte);
        parameters.Add("@ModifiedAt", leagueTeamPlayerInjury.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", leagueTeamPlayerInjury.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateLeagueTeamPlayerInjury",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes an injury by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the injury to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteLeagueTeamPlayerInjury",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves an injury by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the injury to retrieve.</param>
    /// <returns>The matching injury, or <see langword="null"/> when none exists.</returns>
    public async Task<LeagueTeamPlayerInjury?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeamPlayerInjury>(
            "dbo.GetLeagueTeamPlayerInjuryById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every injury of the given squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row whose injuries to retrieve.</param>
    /// <returns>A list of the squad row's injuries.</returns>
    public async Task<List<LeagueTeamPlayerInjury>> GetByLeagueTeamPlayerIdAsync(int leagueTeamPlayerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerId, DbType.Int32);

        var leagueTeamPlayerInjuries = await _dapperDataContext.Connection.QueryAsync<LeagueTeamPlayerInjury>(
            "dbo.GetLeagueTeamPlayerInjuries",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return leagueTeamPlayerInjuries.ToList();
    }
}
