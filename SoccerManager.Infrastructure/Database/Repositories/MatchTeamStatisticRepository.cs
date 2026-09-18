using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="MatchTeamStatistic"/> entities using Dapper.
/// </summary>
public class MatchTeamStatisticRepository : IMatchTeamStatisticRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchTeamStatisticRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public MatchTeamStatisticRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new match team statistic and returns its generated identifier.
    /// </summary>
    /// <param name="matchTeamStatistic">The match team statistic to create.</param>
    /// <returns>The identifier assigned to the created match team statistic.</returns>
    public async Task<int> CreateAsync(MatchTeamStatistic matchTeamStatistic)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TeamId", matchTeamStatistic.TeamId, DbType.Int32);
        parameters.Add("@MatchId", matchTeamStatistic.MatchId, DbType.Int32);
        parameters.Add("@IsHomeTeam", matchTeamStatistic.IsHomeTeam, DbType.Boolean);
        parameters.Add("@ShotsTotal", matchTeamStatistic.ShotsTotal, DbType.Int32);
        parameters.Add("@ShotsOnTarget", matchTeamStatistic.ShotsOnTarget, DbType.Int32);
        parameters.Add("@Possession", matchTeamStatistic.Possession, DbType.Int32);
        parameters.Add("@CreatedAt", matchTeamStatistic.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", matchTeamStatistic.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", matchTeamStatistic.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", matchTeamStatistic.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertMatchTeamStatistic",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing match team statistic.
    /// </summary>
    /// <param name="matchTeamStatistic">The match team statistic with updated values.</param>
    /// <returns>The identifier of the updated match team statistic.</returns>
    public async Task<int> UpdateAsync(MatchTeamStatistic matchTeamStatistic)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", matchTeamStatistic.Id, DbType.Int32);
        parameters.Add("@TeamId", matchTeamStatistic.TeamId, DbType.Int32);
        parameters.Add("@MatchId", matchTeamStatistic.MatchId, DbType.Int32);
        parameters.Add("@IsHomeTeam", matchTeamStatistic.IsHomeTeam, DbType.Boolean);
        parameters.Add("@ShotsTotal", matchTeamStatistic.ShotsTotal, DbType.Int32);
        parameters.Add("@ShotsOnTarget", matchTeamStatistic.ShotsOnTarget, DbType.Int32);
        parameters.Add("@Possession", matchTeamStatistic.Possession, DbType.Int32);
        parameters.Add("@ModifiedAt", matchTeamStatistic.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", matchTeamStatistic.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateMatchTeamStatistic",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a match team statistic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team statistic to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatchTeamStatistic",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a match team statistic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team statistic to retrieve.</param>
    /// <returns>The matching match team statistic, or <see langword="null"/> when none exists.</returns>
    public async Task<MatchTeamStatistic?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<MatchTeamStatistic>(
            "dbo.GetMatchTeamStatisticById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every team statistic recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose team statistics to retrieve.</param>
    /// <returns>A list of the match's team statistics.</returns>
    public async Task<List<MatchTeamStatistic>> GetByMatchIdAsync(int matchId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);

        var matchTeamStatistics = await _dapperDataContext.Connection.QueryAsync<MatchTeamStatistic>(
            "dbo.GetMatchTeamStatistics",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return matchTeamStatistics.ToList();
    }
}
