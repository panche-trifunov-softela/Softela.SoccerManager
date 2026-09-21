using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.MatchFormationPlayerPosition.CreateMatchFormationPlayerPosition;
using SoccerManager.Application.Commands.MatchFormationPlayerPosition.DeleteMatchFormationPlayerPosition;
using SoccerManager.Application.Commands.MatchFormationPlayerPosition.UpdateMatchFormationPlayerPosition;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositionById;
using SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositions;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for match formation player positions, the per-match lineup of each team.
/// </summary>
[ApiController]
[Authorize]
[Route("api/match-formation-player-positions")]
public class MatchFormationPlayerPositionController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchFormationPlayerPositionController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public MatchFormationPlayerPositionController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every lineup slot recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of lineup slots recorded for the match.</returns>
    [HttpGet]
    public async Task<IActionResult> GetMatchFormationPlayerPositions([FromQuery] int matchId, CancellationToken cancellationToken)
    {
        // matchId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetMatchFormationPlayerPositionsRequest { MatchId = matchId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the match formation player position with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match formation player position to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching match formation player position.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMatchFormationPlayerPositionById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetMatchFormationPlayerPositionByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new match formation player position.
    /// </summary>
    /// <param name="request">The match formation player position to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created match formation player position.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMatchFormationPlayerPosition([FromBody] CreateMatchFormationPlayerPositionRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateMatchFormationPlayerPositionRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetMatchFormationPlayerPositionById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the match formation player position with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match formation player position to update.</param>
    /// <param name="request">The updated match formation player position values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatchFormationPlayerPosition(int id, [FromBody] UpdateMatchFormationPlayerPositionRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateMatchFormationPlayerPositionRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the match formation player position with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match formation player position to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatchFormationPlayerPosition(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteMatchFormationPlayerPositionRequest>(new DeleteMatchFormationPlayerPositionRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
