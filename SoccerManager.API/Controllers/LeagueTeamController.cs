using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SoccerManager.API.RateLimiting;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.LeagueTeam.CreateLeagueTeam;
using SoccerManager.Application.Commands.LeagueTeam.DeleteLeagueTeam;
using SoccerManager.Application.Commands.LeagueTeamPlayerLoan.AnswerLeagueTeamPlayerLoanBid;
using SoccerManager.Application.Commands.LeagueTeamPlayerLoan.PlaceLeagueTeamPlayerLoanBid;
using SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.AnswerLeagueTeamPlayerTransferBid;
using SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.PlaceLeagueTeamPlayerTransferBid;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;
using SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBidders;
using SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBids;
using SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerInjuries;
using SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerProfile;
using SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerTransfers;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamById;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamLoanBids;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamNextMatch;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamProfile;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquad;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquadMembers;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamTransferBids;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeams;
using SoccerManager.Application.Queries.LeagueTeam.SearchLeaguePlayersAndTeams;
using SoccerManager.Application.Queries.LeagueTeam.SearchPlayersInLeague;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes create, read and delete operations for the rosters of leagues, and the placing, reading and answering of
/// transfer and loan bids on the players of a club.
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
    /// Returns one page of the players of the league of the given league team matching the given search, filters and sort
    /// order. Players are every player in the game: the club shown is the one registered at in that league (the league
    /// team's own first), and a player registered at no club of it is a free agent, with no club. A registered player's
    /// value and wage are his in-game ones, a free agent's his real-world ones. Allowed for an operator or the club's
    /// current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team whose league is the one to search.</param>
    /// <param name="searchTerm">Text a player name must contain, ignoring case and accents; omitted or blank means no name filter.</param>
    /// <param name="positionId">The identifier of the main position a player must have.</param>
    /// <param name="teamId">The identifier of the club of the registration shown for a player in the league (the league team's own first, else his lowest); a free agent never matches.</param>
    /// <param name="nationalTeamId">The identifier of the national team a player must have.</param>
    /// <param name="minRating">The lowest rating to include, from 1 to 100.</param>
    /// <param name="maxRating">The highest rating to include, from 1 to 100 and not below <paramref name="minRating"/>.</param>
    /// <param name="sortBy">The column to order by: name, rating, age (ascending is youngest first), value or wage, in any case; rating when omitted.</param>
    /// <param name="sortDescending">Whether to order in descending order; descending when omitted.</param>
    /// <param name="page">The 1-based page to return; the first when omitted.</param>
    /// <param name="pageSize">The number of players per page, from 1 to 100; 25 when omitted.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The page of players, the total match count, and the page and page size applied; a 400 when the id is not positive or a parameter is invalid, a 403 when the caller may not manage the club, or an empty page for an operator's unknown league team.</returns>
    [HttpGet("{id}/league-players")]
    [EnableRateLimiting(RateLimitPolicies.Search)]
    public async Task<IActionResult> SearchPlayersInLeague(
        int id,
        [FromQuery] string? searchTerm,
        [FromQuery] int? positionId,
        [FromQuery] int? teamId,
        [FromQuery] int? nationalTeamId,
        [FromQuery] int? minRating,
        [FromQuery] int? maxRating,
        [FromQuery] string? sortBy,
        [FromQuery] bool? sortDescending,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        // A zero or negative id and an invalid filter, sort or page are rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var request = new SearchPlayersInLeagueRequest
        {
            LeagueTeamId = id,
            SearchTerm = searchTerm,
            PositionId = positionId,
            TeamId = teamId,
            NationalTeamId = nationalTeamId,
            MinRating = minRating,
            MaxRating = maxRating,
            SortBy = sortBy,
            SortDescending = sortDescending,
            Page = page,
            PageSize = pageSize,
        };

        var result = await _queryDispatcher.QueryAsync(request, cancellationToken);

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
    /// Returns a player's accepted and completed transfers and loans in the league of the given league team, including the
    /// transfers he was swapped in, newest season first. The player may be registered at any club of that league, rival
    /// clubs included, and only deals into clubs of that league are listed. Allowed for an operator or the club's current
    /// manager.
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
    /// Returns the pending transfer and loan bids on a player of the given league team, each newest first: the bids on the
    /// player's registration at that club, with the bidding club and its current manager, the fee, the buyback or the option
    /// to buy, and the players a transfer bid offers in a swap. Here <c>{id}</c> is the club that holds the player and
    /// answers the bids. Allowed for an operator or the current manager of the club that holds the player.
    /// </summary>
    /// <param name="id">The identifier of the league team that holds the player.</param>
    /// <param name="playerId">The identifier of the player whose pending bids to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The pending transfer and loan bids, both lists empty when there are none; a 400 when an identifier is not positive, a 403 when the caller may not manage the club, or a 404 when the player is not registered at the club.</returns>
    [HttpGet("{id}/league-players/{playerId}/bids")]
    public async Task<IActionResult> GetLeaguePlayerBids(int id, int playerId, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new GetLeaguePlayerBidsRequest { LeagueTeamId = id, PlayerId = playerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns who has bid for a player in the league of the given league team, accepted deals first, then newest first: the
    /// bidding club, its current manager, the season, the day, whether the bid is a transfer or a loan and whether it is
    /// pending or accepted, never the terms. The player may be registered at any club of that league, rival clubs included.
    /// Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team whose league is the one to look in.</param>
    /// <param name="playerId">The identifier of the player whose bidders to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The player's bidders, empty when there are none; a 400 when an identifier is not positive, a 403 when the caller may not manage the club, or a 404 when the player is not registered at any club of the league.</returns>
    [HttpGet("{id}/league-players/{playerId}/bidders")]
    public async Task<IActionResult> GetLeaguePlayerBidders(int id, int playerId, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new GetLeaguePlayerBiddersRequest { LeagueTeamId = id, PlayerId = playerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Places a pending transfer bid by the given league team for a player of another club of its league, optionally with a
    /// buyback and with up to 2 of the bidding club's own players offered in a swap. The selling club is the club the player
    /// is registered at, and the bid is placed in the league's latest season. A club has one pending bid per player, whether
    /// a transfer or a loan, and a player with an agreed deal takes no bid, nor can he be offered in a swap. Here <c>{id}</c>
    /// is the bidding club, not the club that holds the player. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team that bids.</param>
    /// <param name="playerId">The identifier of the player to bid for.</param>
    /// <param name="request">The bid: the fee, the optional buyback value and seasons, and the swapped squad players.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created transfer, with its location; a 400 when a value is invalid, the player is already in the club's squad or at no club of the league, the league has no season yet, the club already has a pending bid for the player, the player already has an agreed deal ("This player already has an agreed deal."), a swapped player already has an agreed deal ("A swapped player already has an agreed deal."), or a swapped player is not in the club's squad; a 403 when the caller may not manage the club; or a 404 when the league team does not exist, which only an operator can reach.</returns>
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
    /// Answers a pending transfer bid on a player of the given league team by accepting or rejecting it. Here <c>{id}</c> is
    /// the club that holds the player and sells him, not the bidding club. An answer only records the agreement: nobody moves,
    /// and a later step completes accepted deals. Accepting also rejects every other pending bid that involves a player of
    /// the deal: the bids on the sold player, the bids that offer him in a swap, and the bids on the players this bid offers
    /// in its swap. Allowed for an operator or the current manager of the club that holds the player.
    /// </summary>
    /// <param name="id">The identifier of the league team that holds the player.</param>
    /// <param name="playerId">The identifier of the player the bid is for.</param>
    /// <param name="bidId">The identifier of the transfer bid to answer.</param>
    /// <param name="request">The answer: Accepted (2) or Rejected (3).</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the answer was recorded; a 400 when an identifier is not positive, the answer is not Accepted or Rejected, or the bid is no longer available (it does not exist, is no longer pending, is not a bid on this player at this club, the club is not its seller, or an accept is refused because a player of the deal already has an agreed deal); or a 403 when the caller may not manage the club.</returns>
    [HttpPut("{id}/league-players/{playerId}/transfer-bids/{bidId}")]
    public async Task<IActionResult> AnswerLeagueTeamPlayerTransferBid(int id, int playerId, int bidId, [FromBody] AnswerLeagueTeamPlayerTransferBidRequest request, CancellationToken cancellationToken)
    {
        // The route values replace anything the body carries for them. An invalid body or a zero or negative id is
        // rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var command = request with { LeagueTeamId = id, PlayerId = playerId, BidId = bidId };
        var result = await _commandDispatcher.SendAsync<bool, AnswerLeagueTeamPlayerTransferBidRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Places a pending loan bid by the given league team for a player of another club of its league, for 1 or 2 seasons and
    /// optionally with an option to buy. The lending club is the club the player is registered at, the bid is placed in the
    /// league's latest season, and the loan club pays all of the player's wage. A club has one pending bid per player, whether
    /// a transfer or a loan, and a player with an agreed deal takes no bid. Here <c>{id}</c> is the bidding club, not the
    /// club that holds the player. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team that bids.</param>
    /// <param name="playerId">The identifier of the player to bid for.</param>
    /// <param name="request">The bid: the fee, the number of seasons and the optional buy option value.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created loan, with its location; a 400 when a value is invalid, the player is already in the club's squad or at no club of the league, the league has no season yet, the club already has a pending bid for the player, or the player already has an agreed deal ("This player already has an agreed deal."); a 403 when the caller may not manage the club; or a 404 when the league team does not exist, which only an operator can reach.</returns>
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
    /// Answers a pending loan bid on a player of the given league team by accepting or rejecting it. Here <c>{id}</c> is the
    /// club that holds the player and lends him, not the bidding club. An answer only records the agreement: the player
    /// stays, and a later step completes accepted deals. Accepting also rejects every other pending bid on the player: the
    /// other loan bids, the transfer bids on him and the transfer bids that offer him in a swap. Allowed for an operator or
    /// the current manager of the club that holds the player.
    /// </summary>
    /// <param name="id">The identifier of the league team that holds the player.</param>
    /// <param name="playerId">The identifier of the player the bid is for.</param>
    /// <param name="bidId">The identifier of the loan bid to answer.</param>
    /// <param name="request">The answer: Accepted (2) or Rejected (3).</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the answer was recorded; a 400 when an identifier is not positive, the answer is not Accepted or Rejected, or the bid is no longer available (it does not exist, is no longer pending, is not a bid on this player at this club, or an accept is refused because the player already has an agreed deal); or a 403 when the caller may not manage the club.</returns>
    [HttpPut("{id}/league-players/{playerId}/loan-bids/{bidId}")]
    public async Task<IActionResult> AnswerLeagueTeamPlayerLoanBid(int id, int playerId, int bidId, [FromBody] AnswerLeagueTeamPlayerLoanBidRequest request, CancellationToken cancellationToken)
    {
        // The route values replace anything the body carries for them. An invalid body or a zero or negative id is
        // rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var command = request with { LeagueTeamId = id, PlayerId = playerId, BidId = bidId };
        var result = await _commandDispatcher.SendAsync<bool, AnswerLeagueTeamPlayerLoanBidRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Returns the transfer bids of the given league team, both the ones it placed and the ones it received on its own
    /// players: the pending ones first, then the completed ones (accepted or completed), each newest first. Rejected bids
    /// are never returned. Each bid carries the player, the other club and its current manager, the fee, the buyback and
    /// the players a bid offers in a swap. Here <c>{id}</c> is that club. Allowed for an operator or the club's current
    /// manager.
    /// </summary>
    /// <param name="id">The identifier of the league team whose transfer bids to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The pending and completed transfer bids, both lists empty when there are none; a 400 when the id is not positive, a 403 when the caller may not manage the club, or two empty lists for an operator's unknown league team.</returns>
    [HttpGet("{id}/transfer-bids")]
    public async Task<IActionResult> GetLeagueTeamTransferBids(int id, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamTransferBidsRequest { LeagueTeamId = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the loan bids of the given league team, both the ones it placed and the ones it received on its own players:
    /// the pending ones first, then the completed ones (accepted or completed), each newest first. Rejected bids are never
    /// returned. Each bid carries the player, the other club and its current manager, the fee, the length of the loan and
    /// the option to buy. Here <c>{id}</c> is that club. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team whose loan bids to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The pending and completed loan bids, both lists empty when there are none; a 400 when the id is not positive, a 403 when the caller may not manage the club, or two empty lists for an operator's unknown league team.</returns>
    [HttpGet("{id}/loan-bids")]
    public async Task<IActionResult> GetLeagueTeamLoanBids(int id, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline, before the access check.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamLoanBidsRequest { LeagueTeamId = id }, cancellationToken);

        return Ok(result.Data);
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
