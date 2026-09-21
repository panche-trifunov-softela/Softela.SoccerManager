using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="LeagueTeamPlayer"/> entities using Dapper.
/// </summary>
public class LeagueTeamPlayerRepository : ILeagueTeamPlayerRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueTeamPlayerRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new league team player and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayer">The league team player to create.</param>
    /// <returns>The identifier assigned to the created league team player.</returns>
    public async Task<int> CreateAsync(LeagueTeamPlayer leagueTeamPlayer)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueTeamPlayer.LeagueId, DbType.Int32);
        parameters.Add("@TeamId", leagueTeamPlayer.TeamId, DbType.Int32);
        parameters.Add("@PlayerId", leagueTeamPlayer.PlayerId, DbType.Int32);
        parameters.Add("@ContractLength", leagueTeamPlayer.ContractLength, DbType.Int32);
        parameters.Add("@ContractSalaryPerWeek", leagueTeamPlayer.ContractSalaryPerWeek, DbType.Int64);
        parameters.Add("@SquadNumber", leagueTeamPlayer.SquadNumber, DbType.Int32);
        parameters.Add("@FansFavoritePlayer", leagueTeamPlayer.FansFavoritePlayer, DbType.Boolean);
        parameters.Add("@Morale", (byte)leagueTeamPlayer.Morale, DbType.Byte);
        parameters.Add("@TransfermarketValue", leagueTeamPlayer.TransfermarketValue, DbType.Int64);
        parameters.Add("@WantedStarterAppearances", leagueTeamPlayer.WantedStarterAppearances, DbType.Int32);
        parameters.Add("@WantedTotalAppearances", leagueTeamPlayer.WantedTotalAppearances, DbType.Int32);
        parameters.Add("@Condition", leagueTeamPlayer.Condition, DbType.Int32);
        parameters.Add("@IsSuspendedDomesticCompetition", leagueTeamPlayer.IsSuspendedDomesticCompetition, DbType.Boolean);
        parameters.Add("@IsSuspendedContinentalCompetition", leagueTeamPlayer.IsSuspendedContinentalCompetition, DbType.Boolean);
        parameters.Add("@CreatedAt", leagueTeamPlayer.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", leagueTeamPlayer.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", leagueTeamPlayer.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", leagueTeamPlayer.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeagueTeamPlayer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing league team player.
    /// </summary>
    /// <param name="leagueTeamPlayer">The league team player with updated values.</param>
    /// <returns>The identifier of the updated league team player.</returns>
    public async Task<int> UpdateAsync(LeagueTeamPlayer leagueTeamPlayer)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", leagueTeamPlayer.Id, DbType.Int32);
        parameters.Add("@LeagueId", leagueTeamPlayer.LeagueId, DbType.Int32);
        parameters.Add("@TeamId", leagueTeamPlayer.TeamId, DbType.Int32);
        parameters.Add("@PlayerId", leagueTeamPlayer.PlayerId, DbType.Int32);
        parameters.Add("@ContractLength", leagueTeamPlayer.ContractLength, DbType.Int32);
        parameters.Add("@ContractSalaryPerWeek", leagueTeamPlayer.ContractSalaryPerWeek, DbType.Int64);
        parameters.Add("@SquadNumber", leagueTeamPlayer.SquadNumber, DbType.Int32);
        parameters.Add("@FansFavoritePlayer", leagueTeamPlayer.FansFavoritePlayer, DbType.Boolean);
        parameters.Add("@Morale", (byte)leagueTeamPlayer.Morale, DbType.Byte);
        parameters.Add("@TransfermarketValue", leagueTeamPlayer.TransfermarketValue, DbType.Int64);
        parameters.Add("@WantedStarterAppearances", leagueTeamPlayer.WantedStarterAppearances, DbType.Int32);
        parameters.Add("@WantedTotalAppearances", leagueTeamPlayer.WantedTotalAppearances, DbType.Int32);
        parameters.Add("@Condition", leagueTeamPlayer.Condition, DbType.Int32);
        parameters.Add("@IsSuspendedDomesticCompetition", leagueTeamPlayer.IsSuspendedDomesticCompetition, DbType.Boolean);
        parameters.Add("@IsSuspendedContinentalCompetition", leagueTeamPlayer.IsSuspendedContinentalCompetition, DbType.Boolean);
        parameters.Add("@ModifiedAt", leagueTeamPlayer.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", leagueTeamPlayer.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateLeagueTeamPlayer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a league team player by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team player to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteLeagueTeamPlayer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a league team player by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team player to retrieve.</param>
    /// <returns>The matching league team player, or <see langword="null"/> when none exists.</returns>
    public async Task<LeagueTeamPlayer?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeamPlayer>(
            "dbo.GetLeagueTeamPlayerById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every player registration belonging to the given league and team.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="teamId">The identifier of the team to filter by.</param>
    /// <returns>A list of the league and team's player registrations.</returns>
    public async Task<List<LeagueTeamPlayer>> GetByLeagueAndTeamAsync(int leagueId, int teamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);
        parameters.Add("@TeamId", teamId, DbType.Int32);

        var leagueTeamPlayers = await _dapperDataContext.Connection.QueryAsync<LeagueTeamPlayer>(
            "dbo.GetLeagueTeamPlayers",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return leagueTeamPlayers.ToList();
    }
}
