using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeamPlayerTransfer"/> entities.
/// </summary>
public interface ILeagueTeamPlayerTransferRepository
{
    /// <summary>
    /// Persists a new transfer and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerTransfer">The transfer to create.</param>
    /// <returns>The identifier assigned to the created transfer.</returns>
    Task<int> CreateAsync(LeagueTeamPlayerTransfer leagueTeamPlayerTransfer);

    /// <summary>
    /// Persists changes to an existing transfer.
    /// </summary>
    /// <param name="leagueTeamPlayerTransfer">The transfer with updated values.</param>
    /// <returns>The identifier of the updated transfer.</returns>
    Task<int> UpdateAsync(LeagueTeamPlayerTransfer leagueTeamPlayerTransfer);

    /// <summary>
    /// Answers a pending transfer bid on a player of the given club, by accepting or rejecting it. Accepting also rejects
    /// every other pending bid that involves a player of the deal, but moves nobody. It locks the league team rows of both
    /// clubs of the deal, the seller's and the bidder's, in Id order inside the caller's transaction, so two answers whose
    /// deals share a club cannot both pass or wait on each other.
    /// </summary>
    /// <param name="id">The identifier of the transfer bid to answer.</param>
    /// <param name="leagueTeamId">The identifier of the league team that holds the player and sells him.</param>
    /// <param name="playerId">The identifier of the player the bid is for.</param>
    /// <param name="status">The answer: <see cref="TransferStatus.Accepted"/> or <see cref="TransferStatus.Rejected"/>.</param>
    /// <param name="modifiedAt">The UTC date and time to stamp on the answered and rejected bids.</param>
    /// <param name="modifiedBy">The identifier of the user answering, to stamp on the answered and rejected bids.</param>
    /// <returns><see langword="true"/> when the answer was recorded, <see langword="false"/> when the bid does not exist, is no longer pending, is not a bid on this player at this club, the club is not its seller, or an accept is refused because a player of the deal already has an agreed deal.</returns>
    Task<bool> AnswerBidAsync(int id, int leagueTeamId, int playerId, TransferStatus status, DateTime modifiedAt, Guid modifiedBy);

    /// <summary>
    /// Removes a transfer by its identifier; the swap rows of the transfer go with it.
    /// </summary>
    /// <param name="id">The identifier of the transfer to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a transfer by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the transfer to retrieve.</param>
    /// <returns>The matching transfer, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeamPlayerTransfer?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every transfer of a squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row whose transfers to retrieve.</param>
    /// <returns>A list of every transfer of the squad row.</returns>
    Task<List<LeagueTeamPlayerTransfer>> GetByLeagueTeamPlayerIdAsync(int leagueTeamPlayerId);
}
