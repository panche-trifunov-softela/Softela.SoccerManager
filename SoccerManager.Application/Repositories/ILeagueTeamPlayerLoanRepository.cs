using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeamPlayerLoan"/> entities.
/// </summary>
public interface ILeagueTeamPlayerLoanRepository
{
    /// <summary>
    /// Persists a new loan and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerLoan">The loan to create.</param>
    /// <returns>The identifier assigned to the created loan.</returns>
    Task<int> CreateAsync(LeagueTeamPlayerLoan leagueTeamPlayerLoan);

    /// <summary>
    /// Persists changes to an existing loan.
    /// </summary>
    /// <param name="leagueTeamPlayerLoan">The loan with updated values.</param>
    /// <returns>The identifier of the updated loan.</returns>
    Task<int> UpdateAsync(LeagueTeamPlayerLoan leagueTeamPlayerLoan);

    /// <summary>
    /// Answers a pending loan bid on a player of the given club, by accepting or rejecting it. Accepting also rejects every
    /// other pending bid on the player, but moves nobody. It locks the league team rows of both clubs of the deal, the
    /// lender's and the bidder's, in Id order inside the caller's transaction, so two answers whose deals share a club
    /// cannot both pass or wait on each other.
    /// </summary>
    /// <param name="id">The identifier of the loan bid to answer.</param>
    /// <param name="leagueTeamId">The identifier of the league team that holds the player and lends him.</param>
    /// <param name="playerId">The identifier of the player the bid is for.</param>
    /// <param name="status">The answer: <see cref="TransferStatus.Accepted"/> or <see cref="TransferStatus.Rejected"/>.</param>
    /// <param name="modifiedAt">The UTC date and time to stamp on the answered and rejected bids.</param>
    /// <param name="modifiedBy">The identifier of the user answering, to stamp on the answered and rejected bids.</param>
    /// <returns><see langword="true"/> when the answer was recorded, <see langword="false"/> when the bid does not exist, is no longer pending, or is not a bid on this player at this club.</returns>
    Task<bool> AnswerBidAsync(int id, int leagueTeamId, int playerId, TransferStatus status, DateTime modifiedAt, Guid modifiedBy);

    /// <summary>
    /// Removes a loan by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the loan to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a loan by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the loan to retrieve.</param>
    /// <returns>The matching loan, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeamPlayerLoan?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every loan of a squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row whose loans to retrieve.</param>
    /// <returns>A list of every loan of the squad row.</returns>
    Task<List<LeagueTeamPlayerLoan>> GetByLeagueTeamPlayerIdAsync(int leagueTeamPlayerId);
}
