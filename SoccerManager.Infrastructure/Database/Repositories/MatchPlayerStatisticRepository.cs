using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="MatchPlayerStatistic"/> entities using Dapper.
/// </summary>
public class MatchPlayerStatisticRepository : IMatchPlayerStatisticRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchPlayerStatisticRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public MatchPlayerStatisticRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new match player statistic and returns its generated identifier.
    /// </summary>
    /// <param name="matchPlayerStatistic">The match player statistic to create.</param>
    /// <returns>The identifier assigned to the created match player statistic.</returns>
    public async Task<int> CreateAsync(MatchPlayerStatistic matchPlayerStatistic)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PlayerId", matchPlayerStatistic.PlayerId, DbType.Int32);
        parameters.Add("@MatchId", matchPlayerStatistic.MatchId, DbType.Int32);
        parameters.Add("@Rating", matchPlayerStatistic.Rating, DbType.Decimal, precision: 3, scale: 1);
        parameters.Add("@IsStarter", matchPlayerStatistic.IsStarter, DbType.Boolean);
        parameters.Add("@MinutesPlayed", matchPlayerStatistic.MinutesPlayed, DbType.Int32);
        parameters.Add("@Goals", matchPlayerStatistic.Goals, DbType.Int32);
        parameters.Add("@Assists", matchPlayerStatistic.Assists, DbType.Int32);
        parameters.Add("@PenaltiesScored", matchPlayerStatistic.PenaltiesScored, DbType.Int32);
        parameters.Add("@PenaltiesMissed", matchPlayerStatistic.PenaltiesMissed, DbType.Int32);
        parameters.Add("@YellowCards", matchPlayerStatistic.YellowCards, DbType.Int32);
        parameters.Add("@RedCards", matchPlayerStatistic.RedCards, DbType.Int32);
        parameters.Add("@CreatedAt", matchPlayerStatistic.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", matchPlayerStatistic.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", matchPlayerStatistic.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", matchPlayerStatistic.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertMatchPlayerStatistic",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing match player statistic.
    /// </summary>
    /// <param name="matchPlayerStatistic">The match player statistic with updated values.</param>
    /// <returns>The identifier of the updated match player statistic.</returns>
    public async Task<int> UpdateAsync(MatchPlayerStatistic matchPlayerStatistic)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", matchPlayerStatistic.Id, DbType.Int32);
        parameters.Add("@PlayerId", matchPlayerStatistic.PlayerId, DbType.Int32);
        parameters.Add("@MatchId", matchPlayerStatistic.MatchId, DbType.Int32);
        parameters.Add("@Rating", matchPlayerStatistic.Rating, DbType.Decimal, precision: 3, scale: 1);
        parameters.Add("@IsStarter", matchPlayerStatistic.IsStarter, DbType.Boolean);
        parameters.Add("@MinutesPlayed", matchPlayerStatistic.MinutesPlayed, DbType.Int32);
        parameters.Add("@Goals", matchPlayerStatistic.Goals, DbType.Int32);
        parameters.Add("@Assists", matchPlayerStatistic.Assists, DbType.Int32);
        parameters.Add("@PenaltiesScored", matchPlayerStatistic.PenaltiesScored, DbType.Int32);
        parameters.Add("@PenaltiesMissed", matchPlayerStatistic.PenaltiesMissed, DbType.Int32);
        parameters.Add("@YellowCards", matchPlayerStatistic.YellowCards, DbType.Int32);
        parameters.Add("@RedCards", matchPlayerStatistic.RedCards, DbType.Int32);
        parameters.Add("@ModifiedAt", matchPlayerStatistic.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", matchPlayerStatistic.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateMatchPlayerStatistic",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a match player statistic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player statistic to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatchPlayerStatistic",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a match player statistic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player statistic to retrieve.</param>
    /// <returns>The matching match player statistic, or <see langword="null"/> when none exists.</returns>
    public async Task<MatchPlayerStatistic?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<MatchPlayerStatistic>(
            "dbo.GetMatchPlayerStatisticById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every player statistic recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose player statistics to retrieve.</param>
    /// <returns>A list of the match's player statistics.</returns>
    public async Task<List<MatchPlayerStatistic>> GetByMatchIdAsync(int matchId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);

        var matchPlayerStatistics = await _dapperDataContext.Connection.QueryAsync<MatchPlayerStatistic>(
            "dbo.GetMatchPlayerStatistics",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return matchPlayerStatistics.ToList();
    }
}
