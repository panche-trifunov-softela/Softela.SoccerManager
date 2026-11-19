using FluentValidation;

namespace SoccerManager.Application.Services;

/// <summary>
/// Holds the rules a transfer, a loan and a swap share, so each is checked by one definition: the clubs and the season
/// of a deal must exist and belong to the player's league, a swapped player must be one the buying club can give away,
/// and a club's bid must be for a player of another club of its league, in the league's latest season, while the club
/// has no other pending bid for that player and the player, like every player offered in a swap, has no agreed deal.
/// </summary>
public interface IPlayerDealService
{
    /// <summary>
    /// Ensures a deal (a transfer or a loan) moves a player to another club of the player's own league, in a season of that
    /// league. Either club may be a guest. It runs for every caller, operators included, because nothing else keeps a deal
    /// inside one league.
    /// </summary>
    /// <param name="ownLeagueTeamId">The identifier of the league team (the club in its league) the player is registered at.</param>
    /// <param name="toLeagueTeamId">The identifier of the league team (the club in its league) the player goes to.</param>
    /// <param name="seasonId">The identifier of the season the deal takes place in.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the player's club, the destination club or the season does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the destination is the player's own club, a club of another league, or the season belongs to another league.</exception>
    Task EnsureDealIsValidAsync(int ownLeagueTeamId, int toLeagueTeamId, int seasonId);

    /// <summary>
    /// Ensures a swapped player can go with a transfer: he is not the player the transfer sells, and he is in the squad of
    /// the buying club. Either club may be a guest. It runs for every caller, operators included, because nothing else checks
    /// a swap.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferId">The identifier of the transfer the player is swapped in.</param>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row (the player at a club of the league) that is swapped.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the transfer or the swapped player's squad row does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the swapped player is the sold player, or is not in the buying club's squad.</exception>
    Task EnsureSwapIsValidAsync(int leagueTeamPlayerTransferId, int leagueTeamPlayerId);

    /// <summary>
    /// Resolves what a club's bid for a player is aimed at, and ensures the bid can be placed: the player is not in the
    /// bidding club's squad, is registered at another club of the league, the league has a season to bid in, the club has
    /// no pending bid for the player yet, and the player has no agreed deal. The bid is placed in the league's latest
    /// season. Either club may be a guest. It runs for every caller, operators included, and takes a lock on the rows of
    /// the bidding club and of the selling club, so it belongs inside the caller's transaction.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) that bids.</param>
    /// <param name="playerId">The identifier of the player the club bids for.</param>
    /// <returns>The resolved target of the bid.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bidding club does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the player is already in the bidding club's squad, is registered at no club of the league, the league has no season yet, the club already has a pending bid for the player, or the player already has an agreed deal.</exception>
    Task<BidTarget> GetBidTargetAsync(int leagueTeamId, int playerId);

    /// <summary>
    /// Ensures none of the players offered in a swap already has an agreed deal. It takes no lock of its own, so it belongs
    /// inside the caller's transaction after <see cref="GetBidTargetAsync"/>, which has locked the bidding club the
    /// swapped players belong to.
    /// </summary>
    /// <param name="leagueTeamPlayerIds">The identifiers of the squad rows (the players at the bidding club) that are offered in the swap.</param>
    /// <exception cref="ValidationException">Thrown when a swapped player already has an agreed deal.</exception>
    Task EnsureSwapPlayersHaveNoAgreedDealAsync(IReadOnlyCollection<int> leagueTeamPlayerIds);
}
