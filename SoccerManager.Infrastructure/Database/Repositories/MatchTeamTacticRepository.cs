using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="MatchTeamTactic"/> entities using Dapper.
/// </summary>
public class MatchTeamTacticRepository : IMatchTeamTacticRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchTeamTacticRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public MatchTeamTacticRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new match team tactic and returns its generated identifier.
    /// </summary>
    /// <param name="matchTeamTactic">The match team tactic to create.</param>
    /// <returns>The identifier assigned to the created match team tactic.</returns>
    public async Task<int> CreateAsync(MatchTeamTactic matchTeamTactic)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchTeamTactic.MatchId, DbType.Int32);
        parameters.Add("@TeamId", matchTeamTactic.TeamId, DbType.Int32);
        parameters.Add("@Mentality", (byte)matchTeamTactic.Mentality, DbType.Byte);
        parameters.Add("@Tempo", (byte)matchTeamTactic.Tempo, DbType.Byte);
        parameters.Add("@Passing", (byte)matchTeamTactic.Passing, DbType.Byte);
        parameters.Add("@Width", (byte)matchTeamTactic.Width, DbType.Byte);
        parameters.Add("@Pressing", (byte)matchTeamTactic.Pressing, DbType.Byte);
        parameters.Add("@Tackling", (byte)matchTeamTactic.Tackling, DbType.Byte);
        parameters.Add("@AttackingSide", (byte)matchTeamTactic.AttackingSide, DbType.Byte);
        parameters.Add("@PenaltyTakerPlayerId", matchTeamTactic.PenaltyTakerPlayerId, DbType.Int32);
        parameters.Add("@FreeKickTakerPlayerId", matchTeamTactic.FreeKickTakerPlayerId, DbType.Int32);
        parameters.Add("@CornerKickTakerPlayerId", matchTeamTactic.CornerKickTakerPlayerId, DbType.Int32);
        parameters.Add("@CaptainPlayerId", matchTeamTactic.CaptainPlayerId, DbType.Int32);
        parameters.Add("@CreatedAt", matchTeamTactic.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", matchTeamTactic.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", matchTeamTactic.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", matchTeamTactic.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertMatchTeamTactic",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing match team tactic.
    /// </summary>
    /// <param name="matchTeamTactic">The match team tactic with updated values.</param>
    /// <returns>The identifier of the updated match team tactic.</returns>
    public async Task<int> UpdateAsync(MatchTeamTactic matchTeamTactic)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", matchTeamTactic.Id, DbType.Int32);
        parameters.Add("@MatchId", matchTeamTactic.MatchId, DbType.Int32);
        parameters.Add("@TeamId", matchTeamTactic.TeamId, DbType.Int32);
        parameters.Add("@Mentality", (byte)matchTeamTactic.Mentality, DbType.Byte);
        parameters.Add("@Tempo", (byte)matchTeamTactic.Tempo, DbType.Byte);
        parameters.Add("@Passing", (byte)matchTeamTactic.Passing, DbType.Byte);
        parameters.Add("@Width", (byte)matchTeamTactic.Width, DbType.Byte);
        parameters.Add("@Pressing", (byte)matchTeamTactic.Pressing, DbType.Byte);
        parameters.Add("@Tackling", (byte)matchTeamTactic.Tackling, DbType.Byte);
        parameters.Add("@AttackingSide", (byte)matchTeamTactic.AttackingSide, DbType.Byte);
        parameters.Add("@PenaltyTakerPlayerId", matchTeamTactic.PenaltyTakerPlayerId, DbType.Int32);
        parameters.Add("@FreeKickTakerPlayerId", matchTeamTactic.FreeKickTakerPlayerId, DbType.Int32);
        parameters.Add("@CornerKickTakerPlayerId", matchTeamTactic.CornerKickTakerPlayerId, DbType.Int32);
        parameters.Add("@CaptainPlayerId", matchTeamTactic.CaptainPlayerId, DbType.Int32);
        parameters.Add("@ModifiedAt", matchTeamTactic.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", matchTeamTactic.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateMatchTeamTactic",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a match team tactic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team tactic to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatchTeamTactic",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a match team tactic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team tactic to retrieve.</param>
    /// <returns>The matching match team tactic, or <see langword="null"/> when none exists.</returns>
    public async Task<MatchTeamTactic?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<MatchTeamTactic>(
            "dbo.GetMatchTeamTacticById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every team tactic recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose team tactics to retrieve.</param>
    /// <returns>A list of the match's team tactics.</returns>
    public async Task<List<MatchTeamTactic>> GetByMatchIdAsync(int matchId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);

        var matchTeamTactics = await _dapperDataContext.Connection.QueryAsync<MatchTeamTactic>(
            "dbo.GetMatchTeamTactics",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return matchTeamTactics.ToList();
    }
}
