using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.CreateLeagueTeamPlayerTransfer;
using SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.DeleteLeagueTeamPlayerTransfer;
using SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.UpdateLeagueTeamPlayerTransfer;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransferById;
using SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransfers;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for the transfers of squad rows between clubs of a league. Any signed-in user can read them;
/// only an operator can write them.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-team-player-transfers")]
public class LeagueTeamPlayerTransferController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerTransferController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueTeamPlayerTransferController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every transfer of the given squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of transfers of the squad row, empty when there are none; a 400 when the identifier is not positive.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagueTeamPlayerTransfers([FromQuery] int leagueTeamPlayerId, CancellationToken cancellationToken)
    {
        // leagueTeamPlayerId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayerTransfersRequest { LeagueTeamPlayerId = leagueTeamPlayerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the transfer with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the transfer to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching transfer, or a 404 when no such transfer exists.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueTeamPlayerTransferById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayerTransferByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new transfer. The selling club is the club of the transferred squad row; the buying club and the season
    /// must be in that club's league.
    /// </summary>
    /// <param name="request">The transfer to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created transfer; a 400 when a value is invalid, the buying club is the selling club or is not in its league, or the season is not in its league; a 403 when the caller is not an operator; or a 404 when the squad row, the buying club or the season does not exist.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateLeagueTeamPlayerTransfer([FromBody] CreateLeagueTeamPlayerTransferRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamPlayerTransferRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueTeamPlayerTransferById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the transfer with the given identifier. The selling club stays the one stored with the transfer, unless the
    /// request names another squad row.
    /// </summary>
    /// <param name="id">The identifier of the transfer to update.</param>
    /// <param name="request">The updated transfer values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded; a 400 when a value is invalid, the buying club is the selling club or is not in its league, or the season is not in its league; a 403 when the caller is not an operator; or a 404 when the transfer, the squad row, the buying club or the season does not exist.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdateLeagueTeamPlayerTransfer(int id, [FromBody] UpdateLeagueTeamPlayerTransferRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueTeamPlayerTransferRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the transfer with the given identifier; its swap rows go with it.
    /// </summary>
    /// <param name="id">The identifier of the transfer to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded; a 403 when the caller is not an operator, or a 404 when no such transfer exists.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteLeagueTeamPlayerTransfer(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueTeamPlayerTransferRequest>(new DeleteLeagueTeamPlayerTransferRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
