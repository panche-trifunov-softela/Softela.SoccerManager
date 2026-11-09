using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="MatchTeamTacticSubstitution"/> entities using Dapper.
/// </summary>
public class MatchTeamTacticSubstitutionRepository : IMatchTeamTacticSubstitutionRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchTeamTacticSubstitutionRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public MatchTeamTacticSubstitutionRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new match team tactic substitution and returns its generated identifier.
    /// </summary>
    /// <param name="matchTeamTacticSubstitution">The match team tactic substitution to create.</param>
    /// <returns>The identifier assigned to the created match team tactic substitution.</returns>
    public async Task<int> CreateAsync(MatchTeamTacticSubstitution matchTeamTacticSubstitution)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchTeamTacticSubstitution.MatchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", matchTeamTacticSubstitution.LeagueTeamId, DbType.Int32);
        parameters.Add("@MinuteAt", matchTeamTacticSubstitution.MinuteAt, DbType.Int32);
        parameters.Add("@Situation", (byte)matchTeamTacticSubstitution.Situation, DbType.Byte);
        parameters.Add("@Mentality", (byte)matchTeamTacticSubstitution.Mentality, DbType.Byte);
        parameters.Add("@Tempo", (byte)matchTeamTacticSubstitution.Tempo, DbType.Byte);
        parameters.Add("@Passing", (byte)matchTeamTacticSubstitution.Passing, DbType.Byte);
        parameters.Add("@Width", (byte)matchTeamTacticSubstitution.Width, DbType.Byte);
        parameters.Add("@Pressing", (byte)matchTeamTacticSubstitution.Pressing, DbType.Byte);
        parameters.Add("@Tackling", (byte)matchTeamTacticSubstitution.Tackling, DbType.Byte);
        parameters.Add("@AttackingSide", (byte)matchTeamTacticSubstitution.AttackingSide, DbType.Byte);
        parameters.Add("@PenaltyTakerPlayerId", matchTeamTacticSubstitution.PenaltyTakerPlayerId, DbType.Int32);
        parameters.Add("@FreeKickTakerPlayerId", matchTeamTacticSubstitution.FreeKickTakerPlayerId, DbType.Int32);
        parameters.Add("@CornerKickTakerPlayerId", matchTeamTacticSubstitution.CornerKickTakerPlayerId, DbType.Int32);
        parameters.Add("@CaptainPlayerId", matchTeamTacticSubstitution.CaptainPlayerId, DbType.Int32);
        parameters.Add("@CreatedAt", matchTeamTacticSubstitution.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", matchTeamTacticSubstitution.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", matchTeamTacticSubstitution.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", matchTeamTacticSubstitution.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertMatchTeamTacticSubstitution",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a club's substitution tactics for a match, ordered by minute, then situation.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <returns>A list of the club's substitution tactics, empty when it has none.</returns>
    public async Task<List<MatchTeamTacticSubstitution>> GetByMatchAndLeagueTeamAsync(int matchId, int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        var rows = await _dapperDataContext.Connection.QueryAsync<MatchTeamTacticSubstitution>(
            "dbo.GetMatchTeamTacticSubstitutions",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return rows.ToList();
    }

    /// <summary>
    /// Retrieves the minute a club's substitutions for a match share. It locks the club's substitutions for the match until
    /// the current transaction ends, so a concurrent save of the same club waits for it; it must be called inside a
    /// transaction.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <returns>The shared minute, or <see langword="null"/> when the club has no substitutions for the match.</returns>
    public async Task<int?> GetMinuteForUpdateAsync(int matchId, int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        return await _dapperDataContext.Connection.QuerySingleAsync<int?>(
            "dbo.GetMatchTeamTacticSubstitutionMinute",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Changes only the given instructions of a substitution tactic, leaving the others, the roles and the creation fields as
    /// they are.
    /// </summary>
    /// <param name="id">The identifier of the substitution tactic.</param>
    /// <param name="mentality">The new mentality, or <see langword="null"/> to keep it.</param>
    /// <param name="tempo">The new tempo, or <see langword="null"/> to keep it.</param>
    /// <param name="passing">The new passing style, or <see langword="null"/> to keep it.</param>
    /// <param name="width">The new width, or <see langword="null"/> to keep it.</param>
    /// <param name="pressing">The new pressing, or <see langword="null"/> to keep it.</param>
    /// <param name="tackling">The new tackling, or <see langword="null"/> to keep it.</param>
    /// <param name="attackingSide">The new attacking side, or <see langword="null"/> to keep it.</param>
    /// <param name="modifiedAt">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="modifiedBy">The identifier of the user making the change.</param>
    public async Task UpdateInstructionsAsync(int id, Mentality? mentality, Tempo? tempo, Passing? passing, Width? width, Pressing? pressing, Tackling? tackling, AttackingSide? attackingSide, DateTime modifiedAt, Guid modifiedBy)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        parameters.Add("@Mentality", (byte?)mentality, DbType.Byte);
        parameters.Add("@Tempo", (byte?)tempo, DbType.Byte);
        parameters.Add("@Passing", (byte?)passing, DbType.Byte);
        parameters.Add("@Width", (byte?)width, DbType.Byte);
        parameters.Add("@Pressing", (byte?)pressing, DbType.Byte);
        parameters.Add("@Tackling", (byte?)tackling, DbType.Byte);
        parameters.Add("@AttackingSide", (byte?)attackingSide, DbType.Byte);
        parameters.Add("@ModifiedAt", modifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", modifiedBy, DbType.Guid);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.UpdateMatchTeamTacticSubstitutionInstructions",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the four roles of a substitution tactic.
    /// </summary>
    /// <param name="id">The identifier of the substitution tactic.</param>
    /// <param name="penaltyTakerPlayerId">The identifier of the player who takes penalties.</param>
    /// <param name="freeKickTakerPlayerId">The identifier of the player who takes free kicks.</param>
    /// <param name="cornerKickTakerPlayerId">The identifier of the player who takes corner kicks.</param>
    /// <param name="captainPlayerId">The identifier of the player who captains the team.</param>
    /// <param name="modifiedAt">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="modifiedBy">The identifier of the user making the change.</param>
    public async Task UpdateRolesAsync(int id, int penaltyTakerPlayerId, int freeKickTakerPlayerId, int cornerKickTakerPlayerId, int captainPlayerId, DateTime modifiedAt, Guid modifiedBy)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        parameters.Add("@PenaltyTakerPlayerId", penaltyTakerPlayerId, DbType.Int32);
        parameters.Add("@FreeKickTakerPlayerId", freeKickTakerPlayerId, DbType.Int32);
        parameters.Add("@CornerKickTakerPlayerId", cornerKickTakerPlayerId, DbType.Int32);
        parameters.Add("@CaptainPlayerId", captainPlayerId, DbType.Int32);
        parameters.Add("@ModifiedAt", modifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", modifiedBy, DbType.Guid);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.UpdateMatchTeamTacticSubstitutionRoles",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a club's substitution tactic for a match at a minute and situation. No matching row is not an error.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <param name="minuteAt">The minute of the substitution to remove.</param>
    /// <param name="situation">The situation of the substitution to remove.</param>
    public async Task DeleteAsync(int matchId, int leagueTeamId, int minuteAt, Situation situation)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@MatchId", matchId, DbType.Int32);
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@MinuteAt", minuteAt, DbType.Int32);
        parameters.Add("@Situation", (byte)situation, DbType.Byte);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteMatchTeamTacticSubstitution",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
