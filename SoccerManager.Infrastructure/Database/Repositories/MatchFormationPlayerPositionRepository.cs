using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="MatchFormationPlayerPosition"/> entities using Dapper.
/// </summary>
public class MatchFormationPlayerPositionRepository : IMatchFormationPlayerPositionRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchFormationPlayerPositionRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public MatchFormationPlayerPositionRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new match formation player position and returns its generated identifier.
    /// </summary>
    /// <param name="matchFormationPlayerPosition">The match formation player position to create.</param>
    /// <returns>The identifier assigned to the created match formation player position.</returns>
    public async Task<int> CreateAsync(MatchFormationPlayerPosition matchFormationPlayerPosition)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchFormationPlayerPosition.MatchId, DbType.Int32);
        parameters.Add("@TeamId", matchFormationPlayerPosition.TeamId, DbType.Int32);
        parameters.Add("@FormationPositionId", matchFormationPlayerPosition.FormationPositionId, DbType.Int32);
        parameters.Add("@PlayerPositionId", matchFormationPlayerPosition.PlayerPositionId, DbType.Int32);
        parameters.Add("@ConditionOnMatch", matchFormationPlayerPosition.ConditionOnMatch, DbType.Int32);
        parameters.Add("@QualityAtPositionOnMatch", matchFormationPlayerPosition.QualityAtPositionOnMatch, DbType.Int32);
        parameters.Add("@IsSuspended", matchFormationPlayerPosition.IsSuspended, DbType.Boolean);
        parameters.Add("@CreatedAt", matchFormationPlayerPosition.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", matchFormationPlayerPosition.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", matchFormationPlayerPosition.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", matchFormationPlayerPosition.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertMatchFormationPlayerPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing match formation player position.
    /// </summary>
    /// <param name="matchFormationPlayerPosition">The match formation player position with updated values.</param>
    /// <returns>The identifier of the updated match formation player position.</returns>
    public async Task<int> UpdateAsync(MatchFormationPlayerPosition matchFormationPlayerPosition)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", matchFormationPlayerPosition.Id, DbType.Int32);
        parameters.Add("@MatchId", matchFormationPlayerPosition.MatchId, DbType.Int32);
        parameters.Add("@TeamId", matchFormationPlayerPosition.TeamId, DbType.Int32);
        parameters.Add("@FormationPositionId", matchFormationPlayerPosition.FormationPositionId, DbType.Int32);
        parameters.Add("@PlayerPositionId", matchFormationPlayerPosition.PlayerPositionId, DbType.Int32);
        parameters.Add("@ConditionOnMatch", matchFormationPlayerPosition.ConditionOnMatch, DbType.Int32);
        parameters.Add("@QualityAtPositionOnMatch", matchFormationPlayerPosition.QualityAtPositionOnMatch, DbType.Int32);
        parameters.Add("@IsSuspended", matchFormationPlayerPosition.IsSuspended, DbType.Boolean);
        parameters.Add("@ModifiedAt", matchFormationPlayerPosition.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", matchFormationPlayerPosition.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateMatchFormationPlayerPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a match formation player position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match formation player position to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatchFormationPlayerPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a match formation player position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match formation player position to retrieve.</param>
    /// <returns>The matching match formation player position, or <see langword="null"/> when none exists.</returns>
    public async Task<MatchFormationPlayerPosition?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<MatchFormationPlayerPosition>(
            "dbo.GetMatchFormationPlayerPositionById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every lineup slot recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose lineup slots to retrieve.</param>
    /// <returns>A list of every lineup slot recorded for the match.</returns>
    public async Task<List<MatchFormationPlayerPosition>> GetByMatchIdAsync(int matchId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);

        var matchFormationPlayerPositions = await _dapperDataContext.Connection.QueryAsync<MatchFormationPlayerPosition>(
            "dbo.GetMatchFormationPlayerPositions",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return matchFormationPlayerPositions.ToList();
    }
}
