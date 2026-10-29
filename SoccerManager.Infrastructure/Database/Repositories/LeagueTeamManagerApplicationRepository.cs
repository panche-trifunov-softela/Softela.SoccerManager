using System.Data;
using Dapper;
using SoccerManager.Application.Models;
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
        parameters.Add("@LeagueTeamId", application.LeagueTeamId, DbType.Int32);
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
    /// Retrieves every application belonging to the given league roster entry, newest first.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry to filter by.</param>
    /// <returns>A list of every application belonging to the league roster entry.</returns>
    public async Task<List<LeagueTeamManagerApplication>> GetByLeagueTeamIdAsync(int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        var applications = await _dapperDataContext.Connection.QueryAsync<LeagueTeamManagerApplication>(
            "dbo.GetLeagueTeamManagerApplications",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return applications.ToList();
    }

    /// <summary>
    /// Retrieves a user's pending applications for the clubs of a league that have no current manager, newest first.
    /// </summary>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose pending applications are retrieved.</param>
    /// <param name="leagueId">The identifier of the league whose clubs to look through.</param>
    /// <returns>A list of the user's pending applications in the league; empty when the user has no manager profile.</returns>
    public async Task<List<LeagueTeamManagerApplication>> GetPendingByUserAndLeagueAsync(Guid userId, int leagueId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);
        parameters.Add("@LeagueId", leagueId, DbType.Int32);

        var applications = await _dapperDataContext.Connection.QueryAsync<LeagueTeamManagerApplication>(
            "dbo.GetPendingLeagueTeamManagerApplicationsByUserAndLeague",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return applications.ToList();
    }

    /// <summary>
    /// Retrieves a user's pending applications that can still be accepted, in every league, newest first, enriched with league and team names.
    /// An application to a club that has a current manager, or in a league where the user already manages a club, is skipped.
    /// </summary>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose pending applications are retrieved.</param>
    /// <returns>A list of the user's pending applications; empty when the user has no manager profile.</returns>
    public async Task<List<GetPendingLeagueTeamManagerApplicationsByUserIdResult>> GetPendingByUserIdAsync(Guid userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);

        var applications = await _dapperDataContext.Connection.QueryAsync<GetPendingLeagueTeamManagerApplicationsByUserIdResult>(
            "dbo.GetPendingLeagueTeamManagerApplicationsByUserId",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return applications.ToList();
    }

    /// <summary>
    /// Rejects every other pending application that accepting the given one makes moot: the other pending applications for the same club, and the applicant's other pending applications in the club's league.
    /// </summary>
    /// <param name="acceptedApplication">The application that was just accepted, carrying the response date and audit values to stamp on the rejected ones.</param>
    public async Task RejectOtherPendingAsync(LeagueTeamManagerApplication acceptedApplication)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@AcceptedId", acceptedApplication.Id, DbType.Int32);
        parameters.Add("@LeagueTeamId", acceptedApplication.LeagueTeamId, DbType.Int32);
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
