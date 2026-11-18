using System.Data;
using Dapper;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="LeagueTeam"/> entities using Dapper.
/// </summary>
public class LeagueTeamRepository : ILeagueTeamRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public LeagueTeamRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new league team and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeam">The league team to create.</param>
    /// <returns>The identifier assigned to the created league team.</returns>
    public async Task<int> CreateAsync(LeagueTeam leagueTeam)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueTeam.LeagueId, DbType.Int32);
        parameters.Add("@TeamId", leagueTeam.TeamId, DbType.Int32);
        parameters.Add("@CreatedAt", leagueTeam.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", leagueTeam.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", leagueTeam.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", leagueTeam.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertLeagueTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds every team that is not yet on the given league's roster to it as a guest, stamping created and modified
    /// with the given time and user.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to complete.</param>
    /// <param name="createdAt">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="createdBy">The identifier of the user to stamp on the created and modified fields.</param>
    public async Task CreateGuestsAsync(int leagueId, DateTime createdAt, Guid createdBy)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);
        parameters.Add("@CreatedAt", createdAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", createdBy, DbType.Guid);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.InsertGuestLeagueTeams",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds every national team that is not yet on the given league's roster to it, stamping created and modified
    /// with the given time and user. A national team joins the roster to play the league's national competition.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to complete.</param>
    /// <param name="createdAt">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="createdBy">The identifier of the user to stamp on the created and modified fields.</param>
    public async Task CreateNationalTeamsAsync(int leagueId, DateTime createdAt, Guid createdBy)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);
        parameters.Add("@CreatedAt", createdAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", createdBy, DbType.Guid);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.InsertNationalLeagueTeams",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a league team by its identifier, but only when nothing is still attached to the club in
    /// that league. The check and the delete are atomic inside the stored procedure.
    /// </summary>
    /// <param name="id">The identifier of the league team to delete.</param>
    /// <returns>
    /// Whether the league team was removed and, when it was not, which kinds of record still block it.
    /// When nothing is flagged as blocking, the league team did not exist.
    /// </returns>
    public async Task<DeleteLeagueTeamResult> DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QuerySingleAsync<DeleteLeagueTeamResult>(
            "dbo.DeleteLeagueTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a league team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team to retrieve.</param>
    /// <returns>The matching league team, or <see langword="null"/> when none exists.</returns>
    public async Task<LeagueTeam?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeam>(
            "dbo.GetLeagueTeamById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a league team's profile: its identifier, its league's identifier and name, and its team's name and logo.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose profile to retrieve.</param>
    /// <returns>The profile row, or <see langword="null"/> when none exists.</returns>
    public async Task<GetLeagueTeamProfileResult?> GetProfileAsync(int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<GetLeagueTeamProfileResult>(
            "dbo.GetLeagueTeamProfile",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every team on the roster of the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to retrieve.</param>
    /// <returns>A list of the league's teams.</returns>
    public async Task<List<LeagueTeam>> GetByLeagueIdAsync(int leagueId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);

        var leagueTeams = await _dapperDataContext.Connection.QueryAsync<LeagueTeam>(
            "dbo.GetLeagueTeams",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return leagueTeams.ToList();
    }

    /// <summary>
    /// Retrieves the given league's non-guest clubs, ordered by team name, with whether each one has a current manager there
    /// and whether the given user has a pending application for it. Guests are left out, because they can never be managed.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose clubs to look through.</param>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose pending applications are flagged.</param>
    /// <returns>A list of the league's non-guest clubs, empty when the league is unknown.</returns>
    public async Task<List<GetAvailableLeagueTeamsResult>> GetAvailableByLeagueIdAsync(int leagueId, Guid userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);
        parameters.Add("@UserId", userId, DbType.Guid);

        var rows = await _dapperDataContext.Connection.QueryAsync<GetAvailableLeagueTeamsResult>(
            "dbo.GetAvailableLeagueTeams",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return rows.ToList();
    }

    /// <summary>
    /// Retrieves the squad of the given league team, each player with their current rating and main position, ordered by line
    /// and position. Every squad row is returned as stored.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose squad to retrieve.</param>
    /// <returns>A list of the squad's players, empty when the league team is unknown or has no players.</returns>
    public async Task<List<GetLeagueTeamSquadResult>> GetSquadAsync(int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        var rows = await _dapperDataContext.Connection.QueryAsync<GetLeagueTeamSquadResult>(
            "dbo.GetLeagueTeamSquad",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return rows.ToList();
    }

    /// <summary>
    /// Retrieves the squad entries of the given league team, each with the player's date of birth, current rating, main
    /// position and national team, and the entry's squad number, value, weekly wage, morale and condition. The order is
    /// <see cref="GetSquadAsync"/>'s, and every squad row is returned as stored.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose squad to retrieve.</param>
    /// <returns>A list of the squad entries, empty when the league team is unknown or has no players.</returns>
    public async Task<List<GetLeagueTeamSquadMembersResult>> GetSquadMembersAsync(int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        var rows = await _dapperDataContext.Connection.QueryAsync<GetLeagueTeamSquadMembersResult>(
            "dbo.GetLeagueTeamSquadMembers",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return rows.ToList();
    }

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
    public async Task<(int PlayerTotalCount, List<SearchLeaguePlayersResult> Players, int TeamTotalCount, List<SearchLeagueTeamsResult> Teams)> SearchLeaguePlayersAndTeamsAsync(int leagueTeamId, string searchTerm, int take)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@SearchTerm", searchTerm, DbType.String);
        parameters.Add("@Take", take, DbType.Int32);

        // The procedure returns the player count, the players, the team count and the teams, in that order.
        using var results = await _dapperDataContext.Connection.QueryMultipleAsync(
            "dbo.SearchLeaguePlayersAndTeams",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        var playerTotalCount = await results.ReadSingleAsync<int>().ConfigureAwait(false);
        var players = await results.ReadAsync<SearchLeaguePlayersResult>().ConfigureAwait(false);
        var teamTotalCount = await results.ReadSingleAsync<int>().ConfigureAwait(false);
        var teams = await results.ReadAsync<SearchLeagueTeamsResult>().ConfigureAwait(false);

        return (playerTotalCount, players.ToList(), teamTotalCount, teams.ToList());
    }

    /// <summary>
    /// Retrieves a player's profile in the league of the given league team: the player's registration at the
    /// league team itself or, failing that, at another club of the same league (the lowest Id first), with the player's
    /// details and every position the player can play.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose profile to retrieve.</param>
    /// <returns>The registration's header row, or <see langword="null"/> when the player is not registered at any club of the league, and the player's positions, the main position first.</returns>
    public async Task<(GetLeaguePlayerProfileResult? Header, List<GetPlayerProfilePositionsResult> Positions)> GetLeaguePlayerProfileAsync(int leagueTeamId, int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        // The procedure returns the header row first and the positions second.
        using var results = await _dapperDataContext.Connection.QueryMultipleAsync(
            "dbo.GetLeaguePlayerProfile",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        var header = await results.ReadFirstOrDefaultAsync<GetLeaguePlayerProfileResult>().ConfigureAwait(false);
        var positions = await results.ReadAsync<GetPlayerProfilePositionsResult>().ConfigureAwait(false);

        return (header, positions.ToList());
    }

    /// <summary>
    /// Retrieves a player's accepted transfers and loans in the league of the given league team, including the transfers
    /// he was swapped in, newest season first, the players swapped in those transfers, and whether the player is registered
    /// at any club of that league.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose transfers and loans to retrieve.</param>
    /// <returns>Whether the player is registered at any club of the league, and the player's deals and the swapped players of the transfers among them, returned whatever that says.</returns>
    public async Task<(bool IsRegistered, List<GetLeaguePlayerTransfersResult> Transfers, List<GetLeaguePlayerTransferSwapPlayersResult> SwapPlayers)> GetLeaguePlayerTransfersAsync(int leagueTeamId, int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        // The procedure returns the registration flag first, the deals second and the swapped players third.
        using var results = await _dapperDataContext.Connection.QueryMultipleAsync(
            "dbo.GetLeaguePlayerTransfers",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        var isRegistered = await results.ReadFirstAsync<bool>().ConfigureAwait(false);
        var transfers = await results.ReadAsync<GetLeaguePlayerTransfersResult>().ConfigureAwait(false);
        var swapPlayers = await results.ReadAsync<GetLeaguePlayerTransferSwapPlayersResult>().ConfigureAwait(false);

        return (isRegistered, transfers.ToList(), swapPlayers.ToList());
    }

    /// <summary>
    /// Retrieves the pending transfer and loan bids on a player's registration at the given league team, newest first, the
    /// players offered in swaps in those transfer bids, and whether the player is registered at that league team. The
    /// transfer bids are read once, so the swapped players always belong to the bids returned.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team that holds the player.</param>
    /// <param name="playerId">The identifier of the player whose pending bids to retrieve.</param>
    /// <returns>Whether the player is registered at the league team, and the player's pending transfer bids, the players offered in them and the pending loan bids, returned whatever that says.</returns>
    public async Task<(bool IsRegistered, List<GetLeaguePlayerTransferBidsResult> TransferBids, List<GetLeaguePlayerTransferBidSwapPlayersResult> SwapPlayers, List<GetLeaguePlayerLoanBidsResult> LoanBids)> GetLeaguePlayerBidsAsync(int leagueTeamId, int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        // The procedure returns the registration flag first, the transfer bids second, the swapped players third and the loan bids fourth.
        using var results = await _dapperDataContext.Connection.QueryMultipleAsync(
            "dbo.GetLeaguePlayerBids",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        var isRegistered = await results.ReadFirstAsync<bool>().ConfigureAwait(false);
        var transferBids = await results.ReadAsync<GetLeaguePlayerTransferBidsResult>().ConfigureAwait(false);
        var swapPlayers = await results.ReadAsync<GetLeaguePlayerTransferBidSwapPlayersResult>().ConfigureAwait(false);
        var loanBids = await results.ReadAsync<GetLeaguePlayerLoanBidsResult>().ConfigureAwait(false);

        return (isRegistered, transferBids.ToList(), swapPlayers.ToList(), loanBids.ToList());
    }

    /// <summary>
    /// Retrieves the pending and accepted transfer and loan bids on a player at any club of the league of the given league
    /// team, accepted deals first, then newest first, and whether the player is registered at any club of that league. No
    /// term of a bid is read.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose bidders to retrieve.</param>
    /// <returns>Whether the player is registered at any club of the league, and the bids on the player, returned whatever that says.</returns>
    public async Task<(bool IsRegistered, List<GetLeaguePlayerBiddersResult> Bidders)> GetLeaguePlayerBiddersAsync(int leagueTeamId, int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        // The procedure returns the registration flag first and the bids second.
        using var results = await _dapperDataContext.Connection.QueryMultipleAsync(
            "dbo.GetLeaguePlayerBidders",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        var isRegistered = await results.ReadFirstAsync<bool>().ConfigureAwait(false);
        var bidders = await results.ReadAsync<GetLeaguePlayerBiddersResult>().ConfigureAwait(false);

        return (isRegistered, bidders.ToList());
    }

    /// <summary>
    /// Retrieves a player's injuries recorded on the player's registration at a club of the league of the given league
    /// team, newest first, and whether the player is registered at any club of that league.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose injuries to retrieve.</param>
    /// <returns>Whether the player is registered at any club of the league, and the player's injuries, returned whatever that says.</returns>
    public async Task<(bool IsRegistered, List<GetLeaguePlayerInjuriesResult> Injuries)> GetLeaguePlayerInjuriesAsync(int leagueTeamId, int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        // The procedure returns the registration flag first and the injuries second.
        using var results = await _dapperDataContext.Connection.QueryMultipleAsync(
            "dbo.GetLeaguePlayerInjuries",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        var isRegistered = await results.ReadFirstAsync<bool>().ConfigureAwait(false);
        var injuries = await results.ReadAsync<GetLeaguePlayerInjuriesResult>().ConfigureAwait(false);

        return (isRegistered, injuries.ToList());
    }

    /// <summary>
    /// Retrieves the roster entry of the given team in the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to look in.</param>
    /// <param name="teamId">The identifier of the team to look for.</param>
    /// <returns>The matching league team, or <see langword="null"/> when the team is not on the league's roster.</returns>
    public async Task<LeagueTeam?> GetByLeagueAndTeamAsync(int leagueId, int teamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);
        parameters.Add("@TeamId", teamId, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<LeagueTeam>(
            "dbo.GetLeagueTeamByLeagueAndTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves the squad of the given league team as the lineup logic reads it: one row per registered player and position
    /// rating, for the caller to group by player. A player registered twice is read through the oldest registration.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team whose squad to retrieve.</param>
    /// <returns>A list of flat candidate rows, empty when the league team is unknown or has no players.</returns>
    public async Task<List<GetLeagueTeamLineupCandidatesResult>> GetLineupCandidatesAsync(int leagueTeamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);

        var rows = await _dapperDataContext.Connection.QueryAsync<GetLeagueTeamLineupCandidatesResult>(
            "dbo.GetLeagueTeamLineupCandidates",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return rows.ToList();
    }

    /// <summary>
    /// Checks whether the given league team has a pending transfer or loan bid for the given player, whichever club the
    /// player is registered at. It locks the league team's row inside the caller's transaction, so two bids from one club
    /// cannot both pass the check.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) that bids.</param>
    /// <param name="playerId">The identifier of the player the bid is for.</param>
    /// <returns><see langword="true"/> when the club has a pending transfer or loan for the player.</returns>
    public async Task<bool> HasPendingBidAsync(int leagueTeamId, int playerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueTeamId", leagueTeamId, DbType.Int32);
        parameters.Add("@PlayerId", playerId, DbType.Int32);

        return await _dapperDataContext.Connection.QuerySingleAsync<bool>(
            "dbo.GetLeagueTeamHasPendingBid",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
