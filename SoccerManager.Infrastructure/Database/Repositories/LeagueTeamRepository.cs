using System.Data;
using Dapper;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="LeagueTeam"/> entities using Dapper.
/// </summary>
public class LeagueTeamRepository : ILeagueTeamRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueTeamRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new league team and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeam">The league team to create.</param>
    /// <returns>The identifier assigned to the created league team.</returns>
    public async Task<int> CreateAsync(LeagueTeam leagueTeam)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueTeam.LeagueId, DbType.Int32);
        parameters.Add("@TeamId", leagueTeam.TeamId, DbType.Int32);
        parameters.Add("@CreatedAt", leagueTeam.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", leagueTeam.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", leagueTeam.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", leagueTeam.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeagueTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a league team by its identifier, but only when nothing is still attached to the club in
    /// that league. The check and the delete are atomic inside the stored procedure.
    /// </summary>
    /// <param name="id">The identifier of the league team to delete.</param>
    /// <returns>
    /// Whether the league team was removed and, when it was not, which kinds of record still block it.
    /// When nothing is flagged as blocking, the league team did not exist.
    /// </returns>
    public async Task<DeleteLeagueTeamResult> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QuerySingleAsync<DeleteLeagueTeamResult>(
            "dbo.DeleteLeagueTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a league team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team to retrieve.</param>
    /// <returns>The matching league team, or <see langword="null"/> when none exists.</returns>
    public async Task<LeagueTeam?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeam>(
            "dbo.GetLeagueTeamById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every team on the roster of the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to retrieve.</param>
    /// <returns>A list of the league's teams.</returns>
    public async Task<List<LeagueTeam>> GetByLeagueIdAsync(int leagueId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);

        var leagueTeams = await _dapperDataContext.Connection.QueryAsync<LeagueTeam>(
            "dbo.GetLeagueTeams",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return leagueTeams.ToList();
    }

    /// <summary>
    /// Retrieves every club on the given league's roster, ordered by team name, with whether each one has a current manager there
    /// and whether the given user has a pending application for it.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to look through.</param>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose pending applications are flagged.</param>
    /// <returns>A list of the league's clubs, empty when the league is unknown.</returns>
    public async Task<List<GetAvailableLeagueTeamsResult>> GetAvailableByLeagueIdAsync(int leagueId, Guid userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);
        parameters.Add("@UserId", userId, DbType.Guid);

        var rows = await _dapperDataContext.Connection.QueryAsync<GetAvailableLeagueTeamsResult>(
            "dbo.GetAvailableLeagueTeams",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return rows.ToList();
    }

    /// <summary>
    /// Retrieves the squad of the given league team, each player with their current rating and main position, ordered by line
    /// and position. Every squad row is returned as stored.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose squad to retrieve.</param>
    /// <returns>A list of the squad's players, empty when the league team is unknown or has no players.</returns>
    public async Task<List<GetLeagueTeamSquadResult>> GetSquadAsync(int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        var rows = await _dapperDataContext.Connection.QueryAsync<GetLeagueTeamSquadResult>(
            "dbo.GetLeagueTeamSquad",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return rows.ToList();
    }

    /// <summary>
    /// Retrieves the roster entry of the given team in the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to look in.</param>
    /// <param name="teamId">The identifier of the team to look for.</param>
    /// <returns>The matching league team, or <see langword="null"/> when the team is not on the league's roster.</returns>
    public async Task<LeagueTeam?> GetByLeagueAndTeamAsync(int leagueId, int teamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);
        parameters.Add("@TeamId", teamId, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeam>(
            "dbo.GetLeagueTeamByLeagueAndTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
