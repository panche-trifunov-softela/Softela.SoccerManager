using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Position.CreatePosition;
using SoccerManager.Application.Commands.Position.DeletePosition;
using SoccerManager.Application.Commands.Position.UpdatePosition;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Position.GetPositionById;
using SoccerManager.Application.Queries.Position.GetPositions;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for positions.
/// </summary>
[ApiController]
[Authorize]
[Route("api/positions")]
public class PositionController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PositionController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public PositionController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every position.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of positions.</returns>
    [HttpGet]
    public async Task<IActionResult> GetPositions(CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetPositionsRequest(), cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the position with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the position to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching position.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPositionById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetPositionByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new position.
    /// </summary>
    /// <param name="request">The position to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created position.</returns>
    [HttpPost]
    public async Task<IActionResult> CreatePosition([FromBody] CreatePositionRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreatePositionRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetPositionById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the position with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the position to update.</param>
    /// <param name="request">The updated position values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePosition(int id, [FromBody] UpdatePositionRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdatePositionRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the position with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the position to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePosition(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeletePositionRequest>(new DeletePositionRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
