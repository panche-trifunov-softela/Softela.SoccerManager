using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.PenaltyAfterExtraTime.CreatePenaltyAfterExtraTime;
using SoccerManager.Application.Commands.PenaltyAfterExtraTime.DeletePenaltyAfterExtraTime;
using SoccerManager.Application.Commands.PenaltyAfterExtraTime.UpdatePenaltyAfterExtraTime;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltiesAfterExtraTime;
using SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltyAfterExtraTimeById;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for the kicks of a penalty shootout after extra time.
/// </summary>
[ApiController]
[Authorize]
[Route("api/penalties-after-extra-time")]
public class PenaltyAfterExtraTimeController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PenaltyAfterExtraTimeController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public PenaltyAfterExtraTimeController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every shootout kick recorded for the given match, in kick order.
    /// </summary>
    /// <param name="matchId">The identifier of the match to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of shootout kicks recorded for the match, in kick order.</returns>
    [HttpGet]
    public async Task<IActionResult> GetPenaltiesAfterExtraTime([FromQuery] int matchId, CancellationToken cancellationToken)
    {
        // matchId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetPenaltiesAfterExtraTimeRequest { MatchId = matchId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the penalty after extra time with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the penalty after extra time to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching penalty after extra time.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPenaltyAfterExtraTimeById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetPenaltyAfterExtraTimeByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new penalty after extra time.
    /// </summary>
    /// <param name="request">The penalty after extra time to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created penalty after extra time.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreatePenaltyAfterExtraTime([FromBody] CreatePenaltyAfterExtraTimeRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreatePenaltyAfterExtraTimeRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetPenaltyAfterExtraTimeById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the penalty after extra time with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the penalty after extra time to update.</param>
    /// <param name="request">The updated penalty after extra time values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdatePenaltyAfterExtraTime(int id, [FromBody] UpdatePenaltyAfterExtraTimeRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdatePenaltyAfterExtraTimeRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the penalty after extra time with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the penalty after extra time to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeletePenaltyAfterExtraTime(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeletePenaltyAfterExtraTimeRequest>(new DeletePenaltyAfterExtraTimeRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
