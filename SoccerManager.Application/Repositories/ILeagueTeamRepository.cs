using SoccerManager.Application.Models;
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
    /// Retrieves every team on the roster of the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to retrieve.</param>
    /// <returns>A list of every league team belonging to the league.</returns>
    Task<List<LeagueTeam>> GetByLeagueIdAsync(int leagueId);

    /// <summary>
    /// Retrieves every club on the given league's roster, ordered by team name, with whether each one has a current manager there
    /// and whether the given user has a pending application for it.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to look through.</param>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose pending applications are flagged.</param>
    /// <returns>A list of the league's clubs, empty when the league is unknown.</returns>
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
    /// Retrieves a player's profile in the league of the given league team: the player's registration at the
    /// league team itself or, failing that, at another club of the same league (the lowest Id first), with the player's
    /// details and every position the player can play.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose profile to retrieve.</param>
    /// <returns>The registration's header row, or <see langword="null"/> when the player is not registered at any club of the league, and the player's positions, the main position first.</returns>
    Task<(GetLeaguePlayerProfileResult? Header, List<GetPlayerProfilePositionsResult> Positions)> GetLeaguePlayerProfileAsync(int leagueTeamId, int playerId);

    /// <summary>
    /// Retrieves a player's accepted transfers and loans in the league of the given league team, including the transfers
    /// he was swapped in, newest season first, the players swapped in those transfers, and whether the player is registered
    /// at any club of that league.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose transfers and loans to retrieve.</param>
    /// <returns>Whether the player is registered at any club of the league, and the player's deals and the swapped players of the transfers among them, returned whatever that says.</returns>
    Task<(bool IsRegistered, List<GetLeaguePlayerTransfersResult> Transfers, List<GetLeaguePlayerTransferSwapPlayersResult> SwapPlayers)> GetLeaguePlayerTransfersAsync(int leagueTeamId, int playerId);

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
}
