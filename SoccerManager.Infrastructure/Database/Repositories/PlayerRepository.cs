using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Player"/> entities using Dapper.
/// </summary>
public class PlayerRepository : IPlayerRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public PlayerRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new player and returns its generated identifier.
    /// </summary>
    /// <param name="player">The player to create.</param>
    /// <returns>The identifier assigned to the created player.</returns>
    public async Task<int> CreateAsync(Player player)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", player.Name, DbType.String);
        // No DbType here: DynamicParameters only looks up a registered type handler (DateOnlyTypeHandler) when the DbType is null; passing DbType.Date would bypass it.
        parameters.Add("@DateOfBirth", player.DateOfBirth);
        parameters.Add("@Rating", player.Rating, DbType.Int32);
        parameters.Add("@Value", player.Value, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@Wage", player.Wage, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@ImageUrl", player.ImageUrl, DbType.String);
        parameters.Add("@NationalTeamId", player.NationalTeamId, DbType.Int32);
        parameters.Add("@TeamId", player.TeamId, DbType.Int32);
        parameters.Add("@TransfermarktId", player.TransfermarktId, DbType.Int32);
        parameters.Add("@CreatedAt", player.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", player.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", player.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", player.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertPlayer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing player.
    /// </summary>
    /// <param name="player">The player with updated values.</param>
    /// <returns>The identifier of the updated player.</returns>
    public async Task<int> UpdateAsync(Player player)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", player.Id, DbType.Int32);
        parameters.Add("@Name", player.Name, DbType.String);
        // No DbType here: DynamicParameters only looks up a registered type handler (DateOnlyTypeHandler) when the DbType is null; passing DbType.Date would bypass it.
        parameters.Add("@DateOfBirth", player.DateOfBirth);
        parameters.Add("@Rating", player.Rating, DbType.Int32);
        parameters.Add("@Value", player.Value, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@Wage", player.Wage, DbType.Decimal, precision: 18, scale: 2);
        parameters.Add("@ImageUrl", player.ImageUrl, DbType.String);
        parameters.Add("@NationalTeamId", player.NationalTeamId, DbType.Int32);
        parameters.Add("@TeamId", player.TeamId, DbType.Int32);
        parameters.Add("@TransfermarktId", player.TransfermarktId, DbType.Int32);
        parameters.Add("@ModifiedAt", player.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", player.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdatePlayer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a player by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeletePlayer",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a player by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player to retrieve.</param>
    /// <returns>The matching player, or <see langword="null"/> when none exists.</returns>
    public async Task<Player?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Player>(
            "dbo.GetPlayerById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all players.
    /// </summary>
    /// <returns>A list of every player.</returns>
    public async Task<List<Player>> GetAllAsync()
    {
        var players = await _dapperDataContext.Connection.QueryAsync<Player>(
            "dbo.GetPlayers",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return players.ToList();
    }
}
