using System.Data;
using Dapper;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;
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
        parameters.Add("@LeagueTeamId", matchTeamTactic.LeagueTeamId, DbType.Int32);
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
    /// Retrieves a club's seven instructions for a match: the ones saved for it, else the ones the club last saved for an
    /// earlier match, else the defaults. It never writes.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <returns>The club's instructions for the match.</returns>
    public async Task<GetMatchTeamTacticInstructionsResult> GetInstructionsAsync(int matchId, int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        return await _dapperDataContext.Connection.QuerySingleAsync<GetMatchTeamTacticInstructionsResult>(
            "dbo.GetMatchTeamTacticInstructions",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Changes only the given instructions of a club's tactic for a match, leaving the others, the roles and the creation
    /// fields as they are.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <param name="mentality">The new mentality, or <see langword="null"/> to keep it.</param>
    /// <param name="tempo">The new tempo, or <see langword="null"/> to keep it.</param>
    /// <param name="passing">The new passing style, or <see langword="null"/> to keep it.</param>
    /// <param name="width">The new width, or <see langword="null"/> to keep it.</param>
    /// <param name="pressing">The new pressing, or <see langword="null"/> to keep it.</param>
    /// <param name="tackling">The new tackling, or <see langword="null"/> to keep it.</param>
    /// <param name="attackingSide">The new attacking side, or <see langword="null"/> to keep it.</param>
    /// <param name="modifiedAt">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="modifiedBy">The identifier of the user making the change.</param>
    /// <returns>The club's instructions after the change, or <see langword="null"/> when the club has no tactic for the match.</returns>
    public async Task<GetMatchTeamTacticInstructionsResult?> UpdateInstructionsAsync(int matchId, int leagueTeamId, Mentality? mentality, Tempo? tempo, Passing? passing, Width? width, Pressing? pressing, Tackling? tackling, AttackingSide? attackingSide, DateTime modifiedAt, Guid modifiedBy)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@Mentality", (byte?)mentality, DbType.Byte);
        parameters.Add("@Tempo", (byte?)tempo, DbType.Byte);
        parameters.Add("@Passing", (byte?)passing, DbType.Byte);
        parameters.Add("@Width", (byte?)width, DbType.Byte);
        parameters.Add("@Pressing", (byte?)pressing, DbType.Byte);
        parameters.Add("@Tackling", (byte?)tackling, DbType.Byte);
        parameters.Add("@AttackingSide", (byte?)attackingSide, DbType.Byte);
        parameters.Add("@ModifiedAt", modifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", modifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleOrDefaultAsync<GetMatchTeamTacticInstructionsResult>(
            "dbo.UpdateMatchTeamTacticInstructions",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
