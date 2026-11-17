using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SoccerManager.API.RateLimiting;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.LeagueTeam.CreateLeagueTeam;
using SoccerManager.Application.Commands.LeagueTeam.DeleteLeagueTeam;
using SoccerManager.Application.Commands.LeagueTeamPlayerLoan.PlaceLeagueTeamPlayerLoanBid;
using SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.PlaceLeagueTeamPlayerTransferBid;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;
using SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerInjuries;
using SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerProfile;
using SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerTransfers;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamById;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamNextMatch;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamProfile;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquad;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquadMembers;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeams;
using SoccerManager.Application.Queries.LeagueTeam.SearchLeaguePlayersAndTeams;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes create, read and delete operations for the rosters of leagues.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-teams")]
public class LeagueTeamController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueTeamController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every team on the roster of the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of teams on the league's roster.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagueTeams([FromQuery] int leagueId, CancellationToken cancellationToken)
    {
        // leagueId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamsRequest { LeagueId = leagueId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the given league's non-guest clubs, by team name, with whether each one has a current manager there
    /// and whether the caller has a pending application for it. A pending application does not count as a manager.
    /// Guests are left out.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose available clubs to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The league's non-guest clubs, empty for an unknown league.</returns>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableLeagueTeams([FromQuery] int leagueId, CancellationToken cancellationToken)
    {
        // leagueId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetAvailableLeagueTeamsRequest { LeagueId = leagueId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the squad of the given league team, each player with their current rating and main position, ordered by line
    /// and position.
    /// </summary>
    /// <param name="id">The identifier of the league team whose squad to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The squad, empty for an unknown league team.</returns>
    [HttpGet("{id}/players")]
    public async Task<IActionResult> GetLeagueTeamSquad(int id, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamSquadRequest { LeagueTeamId = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the squad entries of the given league team, each player with their date of birth, squad number, main
    /// position, national team and current rating, and the entry's value, weekly wage, morale and condition, ordered by
    /// line and position. Allowed for an operator or a current manager of a club in the same league as the league team.
    /// </summary>
    /// <param name="id">The identifier of the league team whose squad to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The squad entries, empty when the league team has none; a 400 when the id is not positive, a 403 when the caller manages no club of the league team's league, or an empty list for an operator's unknown league team.</returns>
    [HttpGet("{id}/squad")]
    public async Task<IActionResult> GetLeagueTeamSquadMembers(int id, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamSquadMembersRequest { LeagueTeamId = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the profile of the given league team: the league it plays in, and its team's name and logo. Allowed for
    /// every signed-in user, as the league and team names are already public.
    /// </summary>
    /// <param name="id">The identifier of the league team whose profile to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The league team's profile; a 400 when the id is not positive, or a 404 when the league team does not exist.</returns>
    [HttpGet("{id}/profile")]
    public async Task<IActionResult> GetLeagueTeamProfile(int id, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamProfileRequest { LeagueTeamId = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Searches the players and the clubs of the league of the given league team, returning the best 5 of each with how many
    /// match in all. A name matches when it contains the text, ignoring case and accents, and names where the text starts a
    /// word come first. Players are every player in the game, not only those at a club of the league: a player registered
    /// at no club of the league is a free agent, with no club. Clubs are every club of the league, guests included. Allowed
    /// for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team whose league is the one to search.</param>
    /// <param name="searchTerm">Text a name must contain, ignoring case and accents; at least 2 characters once trimmed and at most 100 as sent.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The best matching players and clubs, each group with its total match count; a 400 when the id is not positive or the term is missing, shorter than 2 characters once trimmed or longer than 100 characters as sent, a 403 when the caller may not manage the club, or two empty groups for an operator's unknown league team.</returns>
    [HttpGet("{id}/search")]
    [EnableRateLimiting(RateLimitPolicies.Search)]
    public async Task<IActionResult> SearchLeaguePlayersAndTeams(int id, [FromQuery] string? searchTerm, CancellationToken cancellationToken)
    {
        // A zero or negative id and a missing, too short or too long term are rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new SearchLeaguePlayersAndTeamsRequest { LeagueTeamId = id, SearchTerm = searchTerm }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns a player's profile in the league of the given league team: the player's registration with its
    /// squad number, value, weekly wage, contract, morale, condition and suspension, and every position the player can
    /// play. The player may be registered at any club of that league, rival clubs included, and the league team's own
    /// registration is preferred. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose profile to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The player's profile; a 400 when an identifier is not positive, a 403 when the caller may not manage the club, or a 404 when the player is not registered at any club of the league.</returns>
    [HttpGet("{id}/league-players/{playerId}")]
    public async Task<IActionResult> GetLeaguePlayerProfile(int id, int playerId, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new GetLeaguePlayerProfileRequest { LeagueTeamId = id, PlayerId = playerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns a player's accepted transfers and loans in the league of the given league team, including the transfers he
    /// was swapped in, newest season first. The player may be registered at any club of that league, rival clubs included,
    /// and only deals into clubs of that league are listed. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose transfers and loans to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The player's transfers and loans, empty when there are none; a 400 when an identifier is not positive, a 403 when the caller may not manage the club, or a 404 when the player is not registered at any club of the league.</returns>
    [HttpGet("{id}/league-players/{playerId}/transfers")]
    public async Task<IActionResult> GetLeaguePlayerTransfers(int id, int playerId, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new GetLeaguePlayerTransfersRequest { LeagueTeamId = id, PlayerId = playerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns a player's injuries in the league of the given league team, newest first: the injuries recorded on the
    /// player's registration at a club of that league. The player may be registered at any club of that league, rival clubs
    /// included. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose injuries to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The player's injuries, empty when there are none; a 400 when an identifier is not positive, a 403 when the caller may not manage the club, or a 404 when the player is not registered at any club of the league.</returns>
    [HttpGet("{id}/league-players/{playerId}/injuries")]
    public async Task<IActionResult> GetLeaguePlayerInjuries(int id, int playerId, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new GetLeaguePlayerInjuriesRequest { LeagueTeamId = id, PlayerId = playerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Places a pending transfer bid by the given league team for a player of another club of its league, optionally with a
    /// buyback and with up to 2 of the bidding club's own players offered in a swap. The selling club is the club the player
    /// is registered at, and the bid is placed in the league's latest season. A club has one pending bid per player, whether
    /// a transfer or a loan. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team that bids.</param>
    /// <param name="playerId">The identifier of the player to bid for.</param>
    /// <param name="request">The bid: the fee, the optional buyback value and seasons, and the swapped squad players.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created transfer, with its location; a 400 when a value is invalid, the player is already in the club's squad or at no club of the league, the league has no season yet, the club already has a pending bid for the player, or a swapped player is not in the club's squad; a 403 when the caller may not manage the club; or a 404 when the league team does not exist, which only an operator can reach.</returns>
    [HttpPost("{id}/league-players/{playerId}/transfer-bids")]
    public async Task<IActionResult> PlaceLeagueTeamPlayerTransferBid(int id, int playerId, [FromBody] PlaceLeagueTeamPlayerTransferBidRequest request, CancellationToken cancellationToken)
    {
        // The route values replace anything the body carries for them. An invalid body or a zero or negative id is
        // rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var command = request with { LeagueTeamId = id, PlayerId = playerId };
        var newId = await _commandDispatcher.SendAsync<int, PlaceLeagueTeamPlayerTransferBidRequest>(command, cancellationToken);

        // The overload that names the controller is needed: the transfer is read in another controller, which the three-argument form cannot reach.
        return CreatedAtAction(nameof(LeagueTeamPlayerTransferController.GetLeagueTeamPlayerTransferById), "LeagueTeamPlayerTransfer", new { id = newId }, new { id = newId });
    }

    /// <summary>
    /// Places a pending loan bid by the given league team for a player of another club of its league, for 1 or 2 seasons and
    /// optionally with an option to buy. The lending club is the club the player is registered at, the bid is placed in the
    /// league's latest season, and the loan club pays all of the player's wage. A club has one pending bid per player, whether
    /// a transfer or a loan. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team that bids.</param>
    /// <param name="playerId">The identifier of the player to bid for.</param>
    /// <param name="request">The bid: the fee, the number of seasons and the optional buy option value.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created loan, with its location; a 400 when a value is invalid, the player is already in the club's squad or at no club of the league, the league has no season yet, or the club already has a pending bid for the player; a 403 when the caller may not manage the club; or a 404 when the league team does not exist, which only an operator can reach.</returns>
    [HttpPost("{id}/league-players/{playerId}/loan-bids")]
    public async Task<IActionResult> PlaceLeagueTeamPlayerLoanBid(int id, int playerId, [FromBody] PlaceLeagueTeamPlayerLoanBidRequest request, CancellationToken cancellationToken)
    {
        // The route values replace anything the body carries for them. An invalid body or a zero or negative id is
        // rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var command = request with { LeagueTeamId = id, PlayerId = playerId };
        var newId = await _commandDispatcher.SendAsync<int, PlaceLeagueTeamPlayerLoanBidRequest>(command, cancellationToken);

        // The overload that names the controller is needed: the loan is read in another controller, which the three-argument form cannot reach.
        return CreatedAtAction(nameof(LeagueTeamPlayerLoanController.GetLeagueTeamPlayerLoanById), "LeagueTeamPlayerLoan", new { id = newId }, new { id = newId });
    }

    /// <summary>
    /// Returns the earliest match of the given league team that has not started, with its opponent and the club's lineup
    /// for it. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team whose next match to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The next match, or no content when the club has none to come.</returns>
    [HttpGet("{id}/next-match")]
    public async Task<IActionResult> GetLeagueTeamNextMatch(int id, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamNextMatchRequest { LeagueTeamId = id }, cancellationToken);

        // 204 rather than 200 with a null body: the club having no match to come is not an error.
        return result.Data is null ? NoContent() : Ok(result.Data);
    }

    /// <summary>
    /// Returns the league team with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching league team.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueTeamById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Adds a team to the roster of a league. A league created through POST /api/leagues already holds every team, so
    /// this adds a team created after its league, as a non-guest club without a squad.
    /// </summary>
    /// <param name="request">The league and team to add to the roster.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created league team.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateLeagueTeam([FromBody] CreateLeagueTeamRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueTeamById), new { id }, new { id });
    }

    /// <summary>
    /// Removes the league team with the given identifier from the roster, but only while nothing is
    /// attached to the club in that league. A club that still has a squad, manager appointments, applications,
    /// standings, matches, transfers or loans there is refused with a 400 naming what blocks it.
    /// </summary>
    /// <param name="id">The identifier of the league team to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded; a 400 when something is still attached, or a 404 when no such league team exists.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteLeagueTeam(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueTeamRequest>(new DeleteLeagueTeamRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
