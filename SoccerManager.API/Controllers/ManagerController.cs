using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Manager.CreateManager;
using SoccerManager.Application.Commands.Manager.DeleteManager;
using SoccerManager.Application.Commands.Manager.UpdateManager;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Manager.GetManagerById;
using SoccerManager.Application.Queries.Manager.GetManagers;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for manager profiles.
/// </summary>
[ApiController]
[Authorize]
[Route("api/managers")]
public class ManagerController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagerController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public ManagerController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every manager profile.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of every manager profile.</returns>
    [HttpGet]
    public async Task<IActionResult> GetManagers(CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetManagersRequest(), cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the manager profile with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the manager profile to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching manager profile.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetManagerById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetManagerByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new manager profile.
    /// </summary>
    /// <param name="request">The manager profile to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created manager profile.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateManager([FromBody] CreateManagerRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateManagerRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetManagerById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the manager profile with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the manager profile to update.</param>
    /// <param name="request">The updated manager profile values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateManager(int id, [FromBody] UpdateManagerRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateManagerRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the manager profile with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the manager profile to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteManager(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteManagerRequest>(new DeleteManagerRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
