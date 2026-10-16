using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="PlayerRating"/> entities using Dapper.
/// </summary>
public class PlayerRatingRepository : IPlayerRatingRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerRatingRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public PlayerRatingRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Makes a new rating the player's current one, keeping the previous current rating as history, and returns its identifier.
    /// </summary>
    /// <param name="playerRating">The player rating to create.</param>
    /// <returns>The identifier assigned to the created player rating.</returns>
    public async Task<int> CreateAsync(PlayerRating playerRating)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PlayerId", playerRating.PlayerId, DbType.Int32);
        parameters.Add("@Rating", playerRating.Rating, DbType.Int32);
        parameters.Add("@CreatedAt", playerRating.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", playerRating.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", playerRating.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", playerRating.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertPlayerRating",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Corrects the value of an existing rating.
    /// </summary>
    /// <param name="playerRating">The player rating with the corrected value.</param>
    /// <returns>The identifier of the updated player rating.</returns>
    public async Task<int> UpdateAsync(PlayerRating playerRating)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", playerRating.Id, DbType.Int32);
        parameters.Add("@Rating", playerRating.Rating, DbType.Int32);
        parameters.Add("@ModifiedAt", playerRating.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", playerRating.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdatePlayerRating",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a player rating by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player rating to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeletePlayerRating",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a player rating by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the player rating to retrieve.</param>
    /// <returns>The matching player rating, or <see langword="null"/> when none exists.</returns>
    public async Task<PlayerRating?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<PlayerRating>(
            "dbo.GetPlayerRatingById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets every rating the player has had, newest first.
    /// </summary>
    /// <param name="playerId">The identifier of the player whose ratings to retrieve.</param>
    /// <returns>A list of every rating the player has had, newest first.</returns>
    public async Task<List<PlayerRating>> GetByPlayerIdAsync(int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        var playerRatings = await _dapperDataContext.Connection.QueryAsync<PlayerRating>(
            "dbo.GetPlayerRatings",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return playerRatings.ToList();
    }

    /// <summary>
    /// Gets the player's rating in force, or null when there is no such player.
    /// </summary>
    /// <param name="playerId">The identifier of the player whose current rating to retrieve.</param>
    /// <returns>The player's current rating, or <see langword="null"/> when the player does not exist.</returns>
    public async Task<PlayerRating?> GetCurrentByPlayerIdAsync(int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<PlayerRating>(
            "dbo.GetCurrentPlayerRatingByPlayerId",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
