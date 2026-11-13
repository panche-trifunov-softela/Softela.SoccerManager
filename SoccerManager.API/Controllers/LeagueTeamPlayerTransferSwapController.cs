using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.CreateLeagueTeamPlayerTransferSwap;
using SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.DeleteLeagueTeamPlayerTransferSwap;
using SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.UpdateLeagueTeamPlayerTransferSwap;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwapById;
using SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwaps;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for the swapped players of a transfer. Reading is open to any signed-in user;
/// writing is limited to operators.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-team-player-transfer-swaps")]
public class LeagueTeamPlayerTransferSwapController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerTransferSwapController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueTeamPlayerTransferSwapController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every swapped player of the given transfer.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferId">The identifier of the transfer to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of swaps of the transfer; 400 when the identifier is missing or not positive.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagueTeamPlayerTransferSwaps([FromQuery] int leagueTeamPlayerTransferId, CancellationToken cancellationToken)
    {
        // leagueTeamPlayerTransferId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayerTransferSwapsRequest { LeagueTeamPlayerTransferId = leagueTeamPlayerTransferId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the swap with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the swap to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching swap, or 404 when none exists.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueTeamPlayerTransferSwapById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayerTransferSwapByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Adds a swapped player to a transfer. Operators only.
    /// </summary>
    /// <param name="request">The swap to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created swap; 400 when the swapped player is the sold player or is not in the buying club's squad, 403 for a non-operator, 404 when the transfer or the squad row does not exist.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateLeagueTeamPlayerTransferSwap([FromBody] CreateLeagueTeamPlayerTransferSwapRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamPlayerTransferSwapRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueTeamPlayerTransferSwapById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the swap with the given identifier. Operators only.
    /// </summary>
    /// <param name="id">The identifier of the swap to update.</param>
    /// <param name="request">The updated swap values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded; 400 when the swapped player is the sold player or is not in the buying club's squad, 403 for a non-operator, 404 when the swap, the transfer or the squad row does not exist.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdateLeagueTeamPlayerTransferSwap(int id, [FromBody] UpdateLeagueTeamPlayerTransferSwapRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueTeamPlayerTransferSwapRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the swap with the given identifier. Operators only.
    /// </summary>
    /// <param name="id">The identifier of the swap to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded; 403 for a non-operator, 404 when the swap does not exist.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteLeagueTeamPlayerTransferSwap(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueTeamPlayerTransferSwapRequest>(new DeleteLeagueTeamPlayerTransferSwapRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
