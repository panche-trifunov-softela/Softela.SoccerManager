using System.Data;
using Dapper;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="LeagueTeamManager"/> entities using Dapper.
/// </summary>
public class LeagueTeamManagerRepository : ILeagueTeamManagerRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamManagerRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueTeamManagerRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new league team manager and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamManager">The league team manager to create.</param>
    /// <returns>The identifier assigned to the created league team manager.</returns>
    public async Task<int> CreateAsync(LeagueTeamManager leagueTeamManager)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueTeamManager.LeagueId, DbType.Int32);
        parameters.Add("@TeamId", leagueTeamManager.TeamId, DbType.Int32);
        parameters.Add("@ManagerId", leagueTeamManager.ManagerId, DbType.Int32);
        // No DbType here: DynamicParameters only looks up a registered type handler (DateOnlyTypeHandler) when the DbType is null; passing DbType.Date would bypass it.
        parameters.Add("@StartDate", leagueTeamManager.StartDate);
        parameters.Add("@EndDate", leagueTeamManager.EndDate);
        parameters.Add("@IsCurrent", leagueTeamManager.IsCurrent, DbType.Boolean);
        parameters.Add("@CreatedAt", leagueTeamManager.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", leagueTeamManager.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", leagueTeamManager.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", leagueTeamManager.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeagueTeamManager",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing league team manager.
    /// </summary>
    /// <param name="leagueTeamManager">The league team manager with updated values.</param>
    /// <returns>The identifier of the updated league team manager.</returns>
    public async Task<int> UpdateAsync(LeagueTeamManager leagueTeamManager)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", leagueTeamManager.Id, DbType.Int32);
        parameters.Add("@LeagueId", leagueTeamManager.LeagueId, DbType.Int32);
        parameters.Add("@TeamId", leagueTeamManager.TeamId, DbType.Int32);
        parameters.Add("@ManagerId", leagueTeamManager.ManagerId, DbType.Int32);
        // No DbType here: DynamicParameters only looks up a registered type handler (DateOnlyTypeHandler) when the DbType is null; passing DbType.Date would bypass it.
        parameters.Add("@StartDate", leagueTeamManager.StartDate);
        parameters.Add("@EndDate", leagueTeamManager.EndDate);
        parameters.Add("@IsCurrent", leagueTeamManager.IsCurrent, DbType.Boolean);
        parameters.Add("@ModifiedAt", leagueTeamManager.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", leagueTeamManager.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateLeagueTeamManager",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a league team manager by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteLeagueTeamManager",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a league team manager by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager to retrieve.</param>
    /// <returns>The matching league team manager, or <see langword="null"/> when none exists.</returns>
    public async Task<LeagueTeamManager?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeamManager>(
            "dbo.GetLeagueTeamManagerById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every manager appointment belonging to the given league and team.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="teamId">The identifier of the team to filter by.</param>
    /// <returns>A list of the league and team's manager appointments.</returns>
    public async Task<List<LeagueTeamManager>> GetByLeagueAndTeamAsync(int leagueId, int teamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);
        parameters.Add("@TeamId", teamId, DbType.Int32);

        var leagueTeamManagers = await _dapperDataContext.Connection.QueryAsync<LeagueTeamManager>(
            "dbo.GetLeagueTeamManagers",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return leagueTeamManagers.ToList();
    }

    /// <summary>
    /// Retrieves every manager appointment belonging to the manager profile linked to the given Keycloak user, enriched with league and team names.
    /// </summary>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose appointments are retrieved.</param>
    /// <param name="currentOnly">When <see langword="true"/>, narrows the results to appointments where <c>IsCurrent</c> is <see langword="true"/>.</param>
    /// <returns>A list of the user's manager appointments, or an empty list when the user has no manager profile.</returns>
    public async Task<List<GetLeagueTeamManagersByUserIdResult>> GetByUserIdAsync(Guid userId, bool currentOnly)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);
        parameters.Add("@CurrentOnly", currentOnly, DbType.Boolean);

        var myLeagueTeamManagers = await _dapperDataContext.Connection.QueryAsync<GetLeagueTeamManagersByUserIdResult>(
            "dbo.GetLeagueTeamManagersByUserId",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return myLeagueTeamManagers.ToList();
    }
}
