using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Season"/> entities using Dapper.
/// </summary>
public class SeasonRepository : ISeasonRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SeasonRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public SeasonRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new season and returns its generated identifier.
    /// </summary>
    /// <param name="season">The season to create.</param>
    /// <returns>The identifier assigned to the created season.</returns>
    public async Task<int> CreateAsync(Season season)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", season.LeagueId, DbType.Int32);
        parameters.Add("@SeasonNumber", season.SeasonNumber, DbType.Int32);
        parameters.Add("@CreatedAt", season.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", season.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", season.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", season.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertSeason",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing season.
    /// </summary>
    /// <param name="season">The season with updated values.</param>
    /// <returns>The identifier of the updated season.</returns>
    public async Task<int> UpdateAsync(Season season)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", season.Id, DbType.Int32);
        parameters.Add("@SeasonNumber", season.SeasonNumber, DbType.Int32);
        parameters.Add("@ModifiedAt", season.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", season.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateSeason",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a season by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the season to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteSeason",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a season by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the season to retrieve.</param>
    /// <returns>The matching season, or <see langword="null"/> when none exists.</returns>
    public async Task<Season?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Season>(
            "dbo.GetSeasonById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every season belonging to the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose seasons to retrieve.</param>
    /// <returns>A list of the league's seasons.</returns>
    public async Task<List<Season>> GetByLeagueIdAsync(int leagueId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);

        var seasons = await _dapperDataContext.Connection.QueryAsync<Season>(
            "dbo.GetSeasons",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return seasons.ToList();
    }
}
