using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.FormationPosition.CreateFormationPosition;
using SoccerManager.Application.Commands.FormationPosition.DeleteFormationPosition;
using SoccerManager.Application.Commands.FormationPosition.UpdateFormationPosition;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.FormationPosition.GetFormationPositionById;
using SoccerManager.Application.Queries.FormationPosition.GetFormationPositions;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for formation position slots.
/// </summary>
[ApiController]
[Authorize]
[Route("api/formation-positions")]
public class FormationPositionController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormationPositionController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public FormationPositionController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every position slot belonging to the given formation.
    /// </summary>
    /// <param name="formationId">The identifier of the formation to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of position slots belonging to the formation.</returns>
    [HttpGet]
    public async Task<IActionResult> GetFormationPositions([FromQuery] int formationId, CancellationToken cancellationToken)
    {
        // formationId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetFormationPositionsRequest { FormationId = formationId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the formation position slot with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation position slot to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching formation position slot.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFormationPositionById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetFormationPositionByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new formation position slot.
    /// </summary>
    /// <param name="request">The formation position slot to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created formation position slot.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateFormationPosition([FromBody] CreateFormationPositionRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateFormationPositionRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetFormationPositionById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the formation position slot with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation position slot to update.</param>
    /// <param name="request">The updated formation position values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFormationPosition(int id, [FromBody] UpdateFormationPositionRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateFormationPositionRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the formation position slot with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation position slot to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFormationPosition(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteFormationPositionRequest>(new DeleteFormationPositionRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
