using SoccerManager.Application.Models;
using SoccerManager.Application.Models.SearchPlayers;
using SoccerManager.Application.Models.SearchPlayersInLeague;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeam"/> entities.
/// </summary>
public interface ILeagueTeamRepository
{
    /// <summary>
    /// Persists a new league team and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeam">The league team to create.</param>
    /// <returns>The identifier assigned to the created league team.</returns>
    Task<int> CreateAsync(LeagueTeam leagueTeam);

    /// <summary>
    /// Adds every team that is not yet on the given league's roster to it as a guest, stamping created and modified
    /// with the given time and user.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to complete.</param>
    /// <param name="createdAt">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="createdBy">The identifier of the user to stamp on the created and modified fields.</param>
    Task CreateGuestsAsync(int leagueId, DateTime createdAt, Guid createdBy);

    /// <summary>
    /// Adds every national team that is not yet on the given league's roster to it, stamping created and modified
    /// with the given time and user. A national team joins the roster to play the league's national competition.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to complete.</param>
    /// <param name="createdAt">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="createdBy">The identifier of the user to stamp on the created and modified fields.</param>
    Task CreateNationalTeamsAsync(int leagueId, DateTime createdAt, Guid createdBy);

    /// <summary>
    /// Removes a league team by its identifier, but only when nothing is still attached to the club in
    /// that league. The check and the delete are atomic inside the stored procedure.
    /// </summary>
    /// <param name="id">The identifier of the league team to delete.</param>
    /// <returns>
    /// Whether the league team was removed and, when it was not, which kinds of record still block it.
    /// When nothing is flagged as blocking, the league team did not exist.
    /// </returns>
    Task<DeleteLeagueTeamResult> DeleteAsync(int id);

    /// <summary>
    /// Retrieves a league team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team to retrieve.</param>
    /// <returns>The matching league team, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeam?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a league team's profile: its identifier, its league's identifier and name, and its team's name and logo.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose profile to retrieve.</param>
    /// <returns>The profile row, or <see langword="null"/> when none exists.</returns>
    Task<GetLeagueTeamProfileResult?> GetProfileAsync(int leagueTeamId);

    /// <summary>
    /// Retrieves every team on the roster of the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to retrieve.</param>
    /// <returns>A list of every league team belonging to the league.</returns>
    Task<List<LeagueTeam>> GetByLeagueIdAsync(int leagueId);

    /// <summary>
    /// Retrieves the given league's non-guest clubs, ordered by team name, with whether each one has a current manager there
    /// and whether the given user has a pending application for it. Guests are left out, because they can never be managed.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose clubs to look through.</param>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose pending applications are flagged.</param>
    /// <returns>A list of the league's non-guest clubs, empty when the league is unknown.</returns>
    Task<List<GetAvailableLeagueTeamsResult>> GetAvailableByLeagueIdAsync(int leagueId, Guid userId);

    /// <summary>
    /// Retrieves the squad of the given league team, each player with their current rating and main position, ordered by line
    /// and position. Every squad row is returned as stored.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose squad to retrieve.</param>
    /// <returns>A list of the squad's players, empty when the league team is unknown or has no players.</returns>
    Task<List<GetLeagueTeamSquadResult>> GetSquadAsync(int leagueTeamId);

    /// <summary>
    /// Retrieves the squad entries of the given league team, each with the player's date of birth, current rating, main
    /// position and national team, and the entry's squad number, value, weekly wage, morale and condition. The order is
    /// <see cref="GetSquadAsync"/>'s, and every squad row is returned as stored.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose squad to retrieve.</param>
    /// <returns>A list of the squad entries, empty when the league team is unknown or has no players.</returns>
    Task<List<GetLeagueTeamSquadMembersResult>> GetSquadMembersAsync(int leagueTeamId);

    /// <summary>
    /// Searches the players and the clubs of the league of the given league team for the given text, ignoring case and
    /// accents. Players are every player whose name contains it, registered at a club of the league or not: the club shown
    /// is the one registered at in that league (the league team's own first, else the lowest registration), none for a free
    /// agent. Both groups put names where the text starts a word first.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="searchTerm">The text a name must contain, already trimmed.</param>
    /// <param name="take">The most players, and the most clubs, to return.</param>
    /// <returns>The number of matching players and the best of them, then the number of matching clubs and the best of them; all empty or zero when the league team is unknown.</returns>
    Task<(int PlayerTotalCount, List<SearchLeaguePlayersResult> Players, int TeamTotalCount, List<SearchLeagueTeamsResult> Teams)> SearchLeaguePlayersAndTeamsAsync(int leagueTeamId, string searchTerm, int take);

    /// <summary>
    /// Retrieves one page of the players matching a search, seen from the league of the given league team. Players are every
    /// player in the game: the club shown is the one registered at in that league (the league team's own first, else the
    /// lowest registration), none for a free agent. A registered player's value and wage are his in-game ones, a free
    /// agent's his real-world ones, and a player without a current rating is listed, last when ordering by rating.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="criteria">The filters, sort order and page to apply; the club filter matches the club of the registration shown for a player (the league team's own first, else his lowest).</param>
    /// <returns>The requested page of players, empty past the last page, and the total match count; both empty or zero when the league team is unknown.</returns>
    Task<SearchPlayersInLeaguePage> SearchPlayersInLeagueAsync(int leagueTeamId, SearchPlayersCriteria criteria);

    /// <summary>
    /// Retrieves a player's profile in the league of the given league team: the player's registration at the
    /// league team itself or, failing that, at another club of the same league (the lowest Id first), with the player's
    /// details and every position the player can play.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose profile to retrieve.</param>
    /// <returns>The registration's header row, or <see langword="null"/> when the player is not registered at any club of the league, and the player's positions, the main position first.</returns>
    Task<(GetLeaguePlayerProfileResult? Header, List<GetPlayerProfilePositionsResult> Positions)> GetLeaguePlayerProfileAsync(int leagueTeamId, int playerId);

    /// <summary>
    /// Retrieves a player's accepted and completed transfers and loans in the league of the given league team, including
    /// the transfers he was swapped in, newest season first, the players swapped in those transfers, and whether the player
    /// is registered at any club of that league.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose transfers and loans to retrieve.</param>
    /// <returns>Whether the player is registered at any club of the league, and the player's deals and the swapped players of the transfers among them, returned whatever that says.</returns>
    Task<(bool IsRegistered, List<GetLeaguePlayerTransfersResult> Transfers, List<GetLeaguePlayerTransferSwapPlayersResult> SwapPlayers)> GetLeaguePlayerTransfersAsync(int leagueTeamId, int playerId);

    /// <summary>
    /// Retrieves the pending transfer and loan bids on a player's registration at the given league team, newest first, the
    /// players offered in swaps in those transfer bids, and whether the player is registered at that league team. The
    /// transfer bids are read once, so the swapped players always belong to the bids returned.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team that holds the player.</param>
    /// <param name="playerId">The identifier of the player whose pending bids to retrieve.</param>
    /// <returns>Whether the player is registered at the league team, and the player's pending transfer bids, the players offered in them and the pending loan bids, returned whatever that says.</returns>
    Task<(bool IsRegistered, List<GetLeaguePlayerTransferBidsResult> TransferBids, List<GetLeaguePlayerTransferBidSwapPlayersResult> SwapPlayers, List<GetLeaguePlayerLoanBidsResult> LoanBids)> GetLeaguePlayerBidsAsync(int leagueTeamId, int playerId);

    /// <summary>
    /// Retrieves the pending and accepted transfer and loan bids on a player at any club of the league of the given league
    /// team, accepted deals first, then newest first, and whether the player is registered at any club of that league. No
    /// term of a bid is read.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose bidders to retrieve.</param>
    /// <returns>Whether the player is registered at any club of the league, and the bids on the player, returned whatever that says.</returns>
    Task<(bool IsRegistered, List<GetLeaguePlayerBiddersResult> Bidders)> GetLeaguePlayerBiddersAsync(int leagueTeamId, int playerId);

    /// <summary>
    /// Retrieves the pending, accepted and completed transfer bids a league team placed and received, pending first, then
    /// newest first, and the players offered in swaps in those bids. Rejected bids are not returned. The bids are read once,
    /// so the swapped players always belong to the bids returned.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose transfer bids to retrieve.</param>
    /// <returns>The league team's transfer bids and the players offered in them, both empty for an unknown league team.</returns>
    Task<(List<GetLeagueTeamTransferBidsResult> Bids, List<GetLeaguePlayerTransferBidSwapPlayersResult> SwapPlayers)> GetLeagueTeamTransferBidsAsync(int leagueTeamId);

    /// <summary>
    /// Retrieves the pending, accepted and completed loan bids a league team placed and received, pending first, then newest
    /// first. Rejected bids are not returned.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose loan bids to retrieve.</param>
    /// <returns>The league team's loan bids, empty for an unknown league team.</returns>
    Task<List<GetLeagueTeamLoanBidsResult>> GetLeagueTeamLoanBidsAsync(int leagueTeamId);

    /// <summary>
    /// Retrieves a player's injuries recorded on the player's registration at a club of the league of the given league
    /// team, newest first, and whether the player is registered at any club of that league.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose injuries to retrieve.</param>
    /// <returns>Whether the player is registered at any club of the league, and the player's injuries, returned whatever that says.</returns>
    Task<(bool IsRegistered, List<GetLeaguePlayerInjuriesResult> Injuries)> GetLeaguePlayerInjuriesAsync(int leagueTeamId, int playerId);

    /// <summary>
    /// Retrieves the roster entry of the given team in the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to look in.</param>
    /// <param name="teamId">The identifier of the team to look for.</param>
    /// <returns>The matching league team, or <see langword="null"/> when the team is not on the league's roster.</returns>
    Task<LeagueTeam?> GetByLeagueAndTeamAsync(int leagueId, int teamId);

    /// <summary>
    /// Retrieves the squad of the given league team as the lineup logic reads it: one row per registered player and position
    /// rating, for the caller to group by player. A player registered twice is read through the oldest registration.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose squad to retrieve.</param>
    /// <returns>A list of flat candidate rows, empty when the league team is unknown or has no players.</returns>
    Task<List<GetLeagueTeamLineupCandidatesResult>> GetLineupCandidatesAsync(int leagueTeamId);

    /// <summary>
    /// Checks whether the given league team has a pending transfer or loan bid for the given player, whichever club the
    /// player is registered at. It locks the rows of the bidding club and of the selling club inside the caller's
    /// transaction, so two bids from one club cannot both pass the check, and a bid and an accept on the same player run
    /// one after the other.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) that bids.</param>
    /// <param name="sellerLeagueTeamId">The identifier of the league team (the club in its league) the player is registered at.</param>
    /// <param name="playerId">The identifier of the player the bid is for.</param>
    /// <returns><see langword="true"/> when the club has a pending transfer or loan for the player.</returns>
    Task<bool> HasPendingBidAsync(int leagueTeamId, int sellerLeagueTeamId, int playerId);

    /// <summary>
    /// Checks whether the given squad row is in an agreed deal: a transfer with the accepted status on it, a swap row of
    /// an accepted transfer naming it, or an accepted loan on it. A completed deal does not count, the player has moved.
    /// It takes no lock, so it is meant to run after <see cref="HasPendingBidAsync"/> has locked the clubs in the same
    /// transaction.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row (the player at a club of the league).</param>
    /// <returns><see langword="true"/> when the squad row is in an agreed deal.</returns>
    Task<bool> HasAgreedDealAsync(int leagueTeamPlayerId);
}
