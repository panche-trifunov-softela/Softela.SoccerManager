using System.Data;
using Dapper;
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
    /// Removes a league team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
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
