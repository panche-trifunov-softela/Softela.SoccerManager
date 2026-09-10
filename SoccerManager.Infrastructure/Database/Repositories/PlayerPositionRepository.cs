using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="PlayerPosition"/> entities using Dapper.
/// </summary>
public class PlayerPositionRepository : IPlayerPositionRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerPositionRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public PlayerPositionRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new player position and returns its generated identifier.
    /// </summary>
    /// <param name="playerPosition">The player position to create.</param>
    /// <returns>The identifier assigned to the created player position.</returns>
    public async Task<int> CreateAsync(PlayerPosition playerPosition)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PlayerId", playerPosition.PlayerId, DbType.Int32);
        parameters.Add("@PositionId", playerPosition.PositionId, DbType.Int32);
        parameters.Add("@Quality", playerPosition.Quality, DbType.Int32);
        parameters.Add("@CreatedAt", playerPosition.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", playerPosition.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", playerPosition.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", playerPosition.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertPlayerPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing player position.
    /// </summary>
    /// <param name="playerPosition">The player position with updated values.</param>
    /// <returns>The identifier of the updated player position.</returns>
    public async Task<int> UpdateAsync(PlayerPosition playerPosition)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", playerPosition.Id, DbType.Int32);
        parameters.Add("@PlayerId", playerPosition.PlayerId, DbType.Int32);
        parameters.Add("@PositionId", playerPosition.PositionId, DbType.Int32);
        parameters.Add("@Quality", playerPosition.Quality, DbType.Int32);
        parameters.Add("@ModifiedAt", playerPosition.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", playerPosition.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdatePlayerPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a player position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player position to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeletePlayerPosition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a player position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player position to retrieve.</param>
    /// <returns>The matching player position, or <see langword="null"/> when none exists.</returns>
    public async Task<PlayerPosition?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<PlayerPosition>(
            "dbo.GetPlayerPositionById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every position rating belonging to the given player.
    /// </summary>
    /// <param name="playerId">The identifier of the player whose position ratings to retrieve.</param>
    /// <returns>A list of the player's position ratings.</returns>
    public async Task<List<PlayerPosition>> GetByPlayerIdAsync(int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        var playerPositions = await _dapperDataContext.Connection.QueryAsync<PlayerPosition>(
            "dbo.GetPlayerPositions",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return playerPositions.ToList();
    }
}
