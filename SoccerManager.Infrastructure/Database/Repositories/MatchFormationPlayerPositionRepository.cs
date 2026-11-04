using System.Data;
using Dapper;
using SoccerManager.Application.Models;
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
        parameters.Add("@LeagueTeamId", matchFormationPlayerPosition.LeagueTeamId, DbType.Int32);
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
    /// Removes a player's lineup slots for a club in matches that have not started. Slots in started matches
    /// are kept as the record of who played.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) whose slots to remove.</param>
    /// <param name="playerId">The identifier of the player whose slots to remove.</param>
    public async Task DeleteUnstartedByLeagueTeamAndPlayerAsync(int leagueTeamId, int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteUnstartedMatchFormationPlayerPositionsByLeagueTeamAndPlayer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a club's lineup rows for a match, each joined to its formation slot and to the player behind its position
    /// rating, in slot order. The rows can span several formations.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose lineup to retrieve.</param>
    /// <returns>A list of the club's lineup rows, empty when it has none.</returns>
    public async Task<List<GetMatchLineupRowsResult>> GetLineupRowsAsync(int matchId, int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        var rows = await _dapperDataContext.Connection.QueryAsync<GetMatchLineupRowsResult>(
            "dbo.GetMatchLineupRows",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return rows.ToList();
    }

    /// <summary>
    /// Removes a club's whole lineup for a match.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose lineup to remove.</param>
    public async Task DeleteByMatchAndLeagueTeamAsync(int matchId, int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatchFormationPlayerPositionsByMatchAndLeagueTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
