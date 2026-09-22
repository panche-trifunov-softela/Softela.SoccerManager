using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Standing"/> entities using Dapper.
/// </summary>
public class StandingRepository : IStandingRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="StandingRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public StandingRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new standing and returns its generated identifier.
    /// </summary>
    /// <param name="standing">The standing to create.</param>
    /// <returns>The identifier assigned to the created standing.</returns>
    public async Task<int> CreateAsync(Standing standing)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CompetitionId", standing.CompetitionId, DbType.Int32);
        parameters.Add("@SeasonId", standing.SeasonId, DbType.Int32);
        parameters.Add("@DivisionId", standing.DivisionId, DbType.Int32);
        parameters.Add("@TeamId", standing.TeamId, DbType.Int32);
        parameters.Add("@Points", standing.Points, DbType.Int32);
        parameters.Add("@GoalsFor", standing.GoalsFor, DbType.Int32);
        parameters.Add("@GoalsAgainst", standing.GoalsAgainst, DbType.Int32);
        parameters.Add("@Wins", standing.Wins, DbType.Int32);
        parameters.Add("@Draws", standing.Draws, DbType.Int32);
        parameters.Add("@Losses", standing.Losses, DbType.Int32);
        parameters.Add("@CreatedAt", standing.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", standing.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", standing.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", standing.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertStanding",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing standing.
    /// </summary>
    /// <param name="standing">The standing with updated values.</param>
    /// <returns>The identifier of the updated standing.</returns>
    public async Task<int> UpdateAsync(Standing standing)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", standing.Id, DbType.Int32);
        parameters.Add("@CompetitionId", standing.CompetitionId, DbType.Int32);
        parameters.Add("@SeasonId", standing.SeasonId, DbType.Int32);
        parameters.Add("@DivisionId", standing.DivisionId, DbType.Int32);
        parameters.Add("@TeamId", standing.TeamId, DbType.Int32);
        parameters.Add("@Points", standing.Points, DbType.Int32);
        parameters.Add("@GoalsFor", standing.GoalsFor, DbType.Int32);
        parameters.Add("@GoalsAgainst", standing.GoalsAgainst, DbType.Int32);
        parameters.Add("@Wins", standing.Wins, DbType.Int32);
        parameters.Add("@Draws", standing.Draws, DbType.Int32);
        parameters.Add("@Losses", standing.Losses, DbType.Int32);
        parameters.Add("@ModifiedAt", standing.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", standing.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateStanding",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a standing by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the standing to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteStanding",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a standing by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the standing to retrieve.</param>
    /// <returns>The matching standing, or <see langword="null"/> when none exists.</returns>
    public async Task<Standing?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Standing>(
            "dbo.GetStandingById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every standing belonging to the given competition, season and division.
    /// </summary>
    /// <param name="competitionId">The identifier of the competition to filter by.</param>
    /// <param name="seasonId">The identifier of the season to filter by.</param>
    /// <param name="divisionId">The identifier of the division to filter by.</param>
    /// <returns>A list of the matching standings.</returns>
    public async Task<List<Standing>> GetByCompetitionSeasonAndDivisionAsync(int competitionId, int seasonId, int divisionId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CompetitionId", competitionId, DbType.Int32);
        parameters.Add("@SeasonId", seasonId, DbType.Int32);
        parameters.Add("@DivisionId", divisionId, DbType.Int32);

        var standings = await _dapperDataContext.Connection.QueryAsync<Standing>(
            "dbo.GetStandings",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return standings.ToList();
    }
}
