using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="ManagerPlayerRatingVote"/> entities using Dapper.
/// </summary>
public class ManagerPlayerRatingVoteRepository : IManagerPlayerRatingVoteRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagerPlayerRatingVoteRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public ManagerPlayerRatingVoteRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new manager player rating vote and returns its generated identifier.
    /// </summary>
    /// <param name="managerPlayerRatingVote">The vote to create.</param>
    /// <returns>The identifier assigned to the created vote.</returns>
    public async Task<int> CreateAsync(ManagerPlayerRatingVote managerPlayerRatingVote)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@ManagerId", managerPlayerRatingVote.ManagerId, DbType.Int32);
        parameters.Add("@PlayerId", managerPlayerRatingVote.PlayerId, DbType.Int32);
        parameters.Add("@RatingAtTheMoment", managerPlayerRatingVote.RatingAtTheMoment, DbType.Int32);
        parameters.Add("@UpOrDown", managerPlayerRatingVote.UpOrDown, DbType.Boolean);
        parameters.Add("@CreatedAt", managerPlayerRatingVote.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", managerPlayerRatingVote.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", managerPlayerRatingVote.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", managerPlayerRatingVote.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertManagerPlayerRatingVote",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the direction of an existing vote.
    /// </summary>
    /// <param name="managerPlayerRatingVote">The vote with its updated direction.</param>
    /// <returns>The identifier of the updated vote.</returns>
    public async Task<int> UpdateAsync(ManagerPlayerRatingVote managerPlayerRatingVote)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", managerPlayerRatingVote.Id, DbType.Int32);
        parameters.Add("@UpOrDown", managerPlayerRatingVote.UpOrDown, DbType.Boolean);
        parameters.Add("@ModifiedAt", managerPlayerRatingVote.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", managerPlayerRatingVote.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateManagerPlayerRatingVote",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a vote by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the vote to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteManagerPlayerRatingVote",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a vote by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the vote to retrieve.</param>
    /// <returns>The matching vote, or <see langword="null"/> when none exists.</returns>
    public async Task<ManagerPlayerRatingVote?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<ManagerPlayerRatingVote>(
            "dbo.GetManagerPlayerRatingVoteById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets every vote on a player, newest first.
    /// </summary>
    /// <param name="playerId">The identifier of the player whose votes to retrieve.</param>
    /// <returns>A list of every vote on the player, newest first.</returns>
    public async Task<List<ManagerPlayerRatingVote>> GetByPlayerIdAsync(int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        var managerPlayerRatingVotes = await _dapperDataContext.Connection.QueryAsync<ManagerPlayerRatingVote>(
            "dbo.GetManagerPlayerRatingVotes",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return managerPlayerRatingVotes.ToList();
    }

    /// <summary>
    /// Gets a manager's vote on a player at one rating, if they cast one.
    /// </summary>
    /// <param name="managerId">The identifier of the manager.</param>
    /// <param name="playerId">The identifier of the player.</param>
    /// <param name="ratingAtTheMoment">The player's rating the vote was cast against.</param>
    /// <returns>The matching vote, or <see langword="null"/> when the manager has not voted at that rating.</returns>
    public async Task<ManagerPlayerRatingVote?> GetByManagerPlayerAndRatingAsync(int managerId, int playerId, int ratingAtTheMoment)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@ManagerId", managerId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);
        parameters.Add("@RatingAtTheMoment", ratingAtTheMoment, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<ManagerPlayerRatingVote>(
            "dbo.GetManagerPlayerRatingVoteByManagerPlayerAndRating",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
