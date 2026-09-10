using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.PlayerPosition.CreatePlayerPosition;
using SoccerManager.Application.Commands.PlayerPosition.DeletePlayerPosition;
using SoccerManager.Application.Commands.PlayerPosition.UpdatePlayerPosition;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositionById;
using SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositions;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for player position ratings.
/// </summary>
[ApiController]
[Authorize]
[Route("api/player-positions")]
public class PlayerPositionController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerPositionController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public PlayerPositionController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every position rating belonging to the given player.
    /// </summary>
    /// <param name="playerId">The identifier of the player to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of position ratings belonging to the player.</returns>
    [HttpGet]
    public async Task<IActionResult> GetPlayerPositions([FromQuery] int playerId, CancellationToken cancellationToken)
    {
        // playerId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetPlayerPositionsRequest { PlayerId = playerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the player position rating with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the player position rating to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching player position rating.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerPositionById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetPlayerPositionByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new player position rating.
    /// </summary>
    /// <param name="request">The player position rating to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created player position rating.</returns>
    [HttpPost]
    public async Task<IActionResult> CreatePlayerPosition([FromBody] CreatePlayerPositionRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreatePlayerPositionRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetPlayerPositionById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the player position rating with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the player position rating to update.</param>
    /// <param name="request">The updated player position values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayerPosition(int id, [FromBody] UpdatePlayerPositionRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdatePlayerPositionRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the player position rating with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the player position rating to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayerPosition(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeletePlayerPositionRequest>(new DeletePlayerPositionRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
