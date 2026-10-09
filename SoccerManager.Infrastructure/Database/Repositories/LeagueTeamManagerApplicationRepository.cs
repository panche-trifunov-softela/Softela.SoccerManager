using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="LeagueTeamManagerApplication"/> entities using Dapper.
/// </summary>
public class LeagueTeamManagerApplicationRepository : ILeagueTeamManagerApplicationRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamManagerApplicationRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueTeamManagerApplicationRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new league team manager application and returns its generated identifier.
    /// </summary>
    /// <param name="application">The league team manager application to create.</param>
    /// <returns>The identifier assigned to the created league team manager application.</returns>
    public async Task<int> CreateAsync(LeagueTeamManagerApplication application)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", application.LeagueId, DbType.Int32);
        parameters.Add("@TeamId", application.TeamId, DbType.Int32);
        parameters.Add("@ManagerId", application.ManagerId, DbType.Int32);
        parameters.Add("@Status", (byte)application.Status, DbType.Byte);
        parameters.Add("@ResponseDate", application.ResponseDate, DbType.DateTime2);
        parameters.Add("@CreatedAt", application.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", application.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", application.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", application.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeagueTeamManagerApplication",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists the answer to an existing league team manager application.
    /// </summary>
    /// <param name="application">The league team manager application with its updated status and response date.</param>
    /// <returns>The identifier of the answered league team manager application, or 0 when it was no longer pending.</returns>
    public async Task<int> UpdateAsync(LeagueTeamManagerApplication application)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", application.Id, DbType.Int32);
        parameters.Add("@Status", (byte)application.Status, DbType.Byte);
        parameters.Add("@ResponseDate", application.ResponseDate, DbType.DateTime2);
        parameters.Add("@ModifiedAt", application.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", application.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateLeagueTeamManagerApplication",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a league team manager application by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager application to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteLeagueTeamManagerApplication",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a league team manager application by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager application to retrieve.</param>
    /// <returns>The matching league team manager application, or <see langword="null"/> when none exists.</returns>
    public async Task<LeagueTeamManagerApplication?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeamManagerApplication>(
            "dbo.GetLeagueTeamManagerApplicationById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every application belonging to the given league and team, newest first.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="teamId">The identifier of the team to filter by.</param>
    /// <returns>A list of every application belonging to the league and team.</returns>
    public async Task<List<LeagueTeamManagerApplication>> GetByLeagueAndTeamAsync(int leagueId, int teamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);
        parameters.Add("@TeamId", teamId, DbType.Int32);

        var applications = await _dapperDataContext.Connection.QueryAsync<LeagueTeamManagerApplication>(
            "dbo.GetLeagueTeamManagerApplications",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return applications.ToList();
    }

    /// <summary>
    /// Rejects every other pending application that accepting the given one makes moot: the other pending applications for the same team in the league, and the applicant's other pending applications in the league.
    /// </summary>
    /// <param name="acceptedApplication">The application that was just accepted, carrying the response date and audit values to stamp on the rejected ones.</param>
    public async Task RejectOtherPendingAsync(LeagueTeamManagerApplication acceptedApplication)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@AcceptedId", acceptedApplication.Id, DbType.Int32);
        parameters.Add("@LeagueId", acceptedApplication.LeagueId, DbType.Int32);
        parameters.Add("@TeamId", acceptedApplication.TeamId, DbType.Int32);
        parameters.Add("@ManagerId", acceptedApplication.ManagerId, DbType.Int32);
        parameters.Add("@ResponseDate", acceptedApplication.ResponseDate, DbType.DateTime2);
        parameters.Add("@ModifiedAt", acceptedApplication.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", acceptedApplication.ModifiedBy, DbType.Guid);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.RejectPendingLeagueTeamManagerApplications",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
