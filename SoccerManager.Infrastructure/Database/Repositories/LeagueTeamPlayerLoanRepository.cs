using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="LeagueTeamPlayerLoan"/> entities using Dapper.
/// </summary>
public class LeagueTeamPlayerLoanRepository : ILeagueTeamPlayerLoanRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerLoanRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueTeamPlayerLoanRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new loan and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerLoan">The loan to create.</param>
    /// <returns>The identifier assigned to the created loan.</returns>
    public async Task<int> CreateAsync(LeagueTeamPlayerLoan leagueTeamPlayerLoan)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerLoan.LeagueTeamPlayerId, DbType.Int32);
        parameters.Add("@ToLeagueTeamId", leagueTeamPlayerLoan.ToLeagueTeamId, DbType.Int32);
        parameters.Add("@SeasonId", leagueTeamPlayerLoan.SeasonId, DbType.Int32);
        parameters.Add("@LoanFee", leagueTeamPlayerLoan.LoanFee, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@SeasonsLength", leagueTeamPlayerLoan.SeasonsLength, DbType.Int32);
        parameters.Add("@BuyOptionValue", leagueTeamPlayerLoan.BuyOptionValue, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@SalaryCoverageInPercent", leagueTeamPlayerLoan.SalaryCoverageInPercent, DbType.Int32);
        parameters.Add("@Status", (byte)leagueTeamPlayerLoan.Status, DbType.Byte);
        parameters.Add("@CreatedAt", leagueTeamPlayerLoan.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", leagueTeamPlayerLoan.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", leagueTeamPlayerLoan.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", leagueTeamPlayerLoan.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeagueTeamPlayerLoan",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing loan.
    /// </summary>
    /// <param name="leagueTeamPlayerLoan">The loan with updated values.</param>
    /// <returns>The identifier of the updated loan.</returns>
    public async Task<int> UpdateAsync(LeagueTeamPlayerLoan leagueTeamPlayerLoan)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", leagueTeamPlayerLoan.Id, DbType.Int32);
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerLoan.LeagueTeamPlayerId, DbType.Int32);
        parameters.Add("@ToLeagueTeamId", leagueTeamPlayerLoan.ToLeagueTeamId, DbType.Int32);
        parameters.Add("@SeasonId", leagueTeamPlayerLoan.SeasonId, DbType.Int32);
        parameters.Add("@LoanFee", leagueTeamPlayerLoan.LoanFee, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@SeasonsLength", leagueTeamPlayerLoan.SeasonsLength, DbType.Int32);
        parameters.Add("@BuyOptionValue", leagueTeamPlayerLoan.BuyOptionValue, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@SalaryCoverageInPercent", leagueTeamPlayerLoan.SalaryCoverageInPercent, DbType.Int32);
        parameters.Add("@Status", (byte)leagueTeamPlayerLoan.Status, DbType.Byte);
        parameters.Add("@ModifiedAt", leagueTeamPlayerLoan.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", leagueTeamPlayerLoan.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateLeagueTeamPlayerLoan",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a loan by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the loan to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteLeagueTeamPlayerLoan",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a loan by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the loan to retrieve.</param>
    /// <returns>The matching loan, or <see langword="null"/> when none exists.</returns>
    public async Task<LeagueTeamPlayerLoan?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeamPlayerLoan>(
            "dbo.GetLeagueTeamPlayerLoanById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every loan of the given squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row whose loans to retrieve.</param>
    /// <returns>A list of the squad row's loans.</returns>
    public async Task<List<LeagueTeamPlayerLoan>> GetByLeagueTeamPlayerIdAsync(int leagueTeamPlayerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamPlayerId", leagueTeamPlayerId, DbType.Int32);

        var leagueTeamPlayerLoans = await _dapperDataContext.Connection.QueryAsync<LeagueTeamPlayerLoan>(
            "dbo.GetLeagueTeamPlayerLoans",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return leagueTeamPlayerLoans.ToList();
    }
}
