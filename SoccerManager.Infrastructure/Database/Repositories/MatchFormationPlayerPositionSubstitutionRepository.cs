using System.Data;
using Dapper;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="MatchFormationPlayerPositionSubstitution"/> entities using Dapper.
/// </summary>
public class MatchFormationPlayerPositionSubstitutionRepository : IMatchFormationPlayerPositionSubstitutionRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchFormationPlayerPositionSubstitutionRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public MatchFormationPlayerPositionSubstitutionRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new match formation player position substitution and returns its generated identifier.
    /// </summary>
    /// <param name="matchFormationPlayerPositionSubstitution">The match formation player position substitution to create.</param>
    /// <returns>The identifier assigned to the created match formation player position substitution.</returns>
    public async Task<int> CreateAsync(MatchFormationPlayerPositionSubstitution matchFormationPlayerPositionSubstitution)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchFormationPlayerPositionSubstitution.MatchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", matchFormationPlayerPositionSubstitution.LeagueTeamId, DbType.Int32);
        parameters.Add("@MinuteAt", matchFormationPlayerPositionSubstitution.MinuteAt, DbType.Int32);
        parameters.Add("@Situation", (byte)matchFormationPlayerPositionSubstitution.Situation, DbType.Byte);
        parameters.Add("@FormationPositionId", matchFormationPlayerPositionSubstitution.FormationPositionId, DbType.Int32);
        parameters.Add("@PlayerPositionId", matchFormationPlayerPositionSubstitution.PlayerPositionId, DbType.Int32);
        parameters.Add("@ConditionOnMatch", matchFormationPlayerPositionSubstitution.ConditionOnMatch, DbType.Int32);
        parameters.Add("@QualityAtPositionOnMatch", matchFormationPlayerPositionSubstitution.QualityAtPositionOnMatch, DbType.Int32);
        parameters.Add("@IsSuspended", matchFormationPlayerPositionSubstitution.IsSuspended, DbType.Boolean);
        parameters.Add("@CreatedAt", matchFormationPlayerPositionSubstitution.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", matchFormationPlayerPositionSubstitution.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", matchFormationPlayerPositionSubstitution.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", matchFormationPlayerPositionSubstitution.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertMatchFormationPlayerPositionSubstitution",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a match formation player position substitution by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match formation player position substitution to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatchFormationPlayerPositionSubstitution",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the whole lineup of one of a club's substitutions for a match.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose substitution lineup to remove.</param>
    /// <param name="minuteAt">The minute of the substitution whose lineup to remove.</param>
    /// <param name="situation">The situation of the substitution whose lineup to remove.</param>
    public async Task DeleteByMinuteAndSituationAsync(int matchId, int leagueTeamId, int minuteAt, Situation situation)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@MinuteAt", minuteAt, DbType.Int32);
        parameters.Add("@Situation", (byte)situation, DbType.Byte);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatchFormationPlayerPositionSubstitutionsByMinuteAndSituation",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a player's slots for a club from its substitution lineups in matches that have not started. Slots in started
    /// matches are kept as the record of what was planned.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) whose slots to remove.</param>
    /// <param name="playerId">The identifier of the player whose slots to remove.</param>
    public async Task DeleteUnstartedByLeagueTeamAndPlayerAsync(int leagueTeamId, int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteUnstartedMatchFormationPlayerPositionSubstitutionsByLeagueTeamAndPlayer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every substitution lineup row of a club for a match, each joined to its formation slot and to the player
    /// behind its position rating, ordered by minute, situation, then slot.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose substitution lineups to retrieve.</param>
    /// <returns>A list of the club's substitution lineup rows, empty when it has none.</returns>
    public async Task<List<GetMatchLineupSubstitutionRowsResult>> GetLineupRowsAsync(int matchId, int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        var rows = await _dapperDataContext.Connection.QueryAsync<GetMatchLineupSubstitutionRowsResult>(
            "dbo.GetMatchLineupSubstitutionRows",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return rows.ToList();
    }
}
